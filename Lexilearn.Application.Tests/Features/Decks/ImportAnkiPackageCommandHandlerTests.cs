using System.Linq.Expressions;
using Lexilearn.Application.Contracts.Infastructure;
using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Lexilearn.Decks.Commands.ImportAnkiPackage;
using Lexilearn.Application.Models.AnkiImport;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using Moq;

namespace Lexilearn.Application.Tests.Features.Decks;

public class ImportAnkiPackageCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IAsyncRepository<Card>> _cardRepository = new();
    private readonly Mock<IAsyncRepository<Deck>> _deckRepository = new();
    private readonly Mock<IAnkiPackageParser> _parser = new();
    private readonly ImportAnkiPackageCommandHandler _sut;

    public ImportAnkiPackageCommandHandlerTests()
    {
        _unitOfWork.Setup(u => u.Repository<Card>()).Returns(_cardRepository.Object);
        _unitOfWork.Setup(u => u.Repository<Deck>()).Returns(_deckRepository.Object);

        var nextDeckId = 1;
        _deckRepository.Setup(r => r.AddAsync(It.IsAny<Deck>()))
            .ReturnsAsync((Deck deck) =>
            {
                deck.Id = nextDeckId++;
                return deck;
            });
        _cardRepository.Setup(r => r.AddAsync(It.IsAny<Card>()))
            .ReturnsAsync((Card card) => card);
        _cardRepository.Setup(r => r.GetMany(It.IsAny<Expression<Func<Card, bool>>>()))
            .ReturnsAsync(new List<Card>());

        _sut = new ImportAnkiPackageCommandHandler(_unitOfWork.Object, _parser.Object);
    }

    [Fact]
    public async Task Handle_ReturnsInvalidPackageError_WhenParserThrows()
    {
        _parser.Setup(p => p.ParseAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AnkiImportException("bad file"));

        var result = await _sut.Handle(CreateCommand(), CancellationToken.None);

        Assert.True(result.HasErrors);
        Assert.Contains(Error.InvalidAnkiPackage.Code, result.Error);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyPackageError_WhenNoDecksParsed()
    {
        _parser.Setup(p => p.ParseAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AnkiPackage());

        var result = await _sut.Handle(CreateCommand(), CancellationToken.None);

        Assert.True(result.HasErrors);
        Assert.Contains(Error.EmptyAnkiPackage.Code, result.Error);
    }

    [Fact]
    public async Task Handle_SkipsCards_AlreadyImportedForTheSameUser()
    {
        var existingCard = new Card
        {
            Front = "old",
            Back = "old",
            ImportSource = "anki",
            ExternalId = "1",
            Deck = new Deck { CreatedBy = 10, Title = "Existing", TermLanguageCode = "en", DefinitionLanguageCode = "es" },
        };
        _cardRepository.Setup(r => r.GetMany(It.IsAny<Expression<Func<Card, bool>>>()))
            .ReturnsAsync(new List<Card> { existingCard });

        _parser.Setup(p => p.ParseAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AnkiPackage
            {
                Decks =
                {
                    new AnkiDeck
                    {
                        Name = "Deck A",
                        Notes =
                        {
                            new AnkiNote { CardExternalId = "1", Front = "dup", Back = "dup" },
                            new AnkiNote { CardExternalId = "2", Front = "new", Back = "new" },
                        },
                    },
                },
            });

        var result = await _sut.Handle(CreateCommand(), CancellationToken.None);

        Assert.False(result.HasErrors);
        Assert.Equal(1, result.Value!.TotalCardsImported);
        Assert.Equal(1, result.Value.TotalCardsSkipped);
        _cardRepository.Verify(
            r => r.AddAsync(It.Is<Card>(c => c.ExternalId == "2" && c.SchedulingState != null)),
            Times.Once);
        _cardRepository.Verify(r => r.AddAsync(It.Is<Card>(c => c.ExternalId == "1")), Times.Never);
    }

    [Fact]
    public async Task Handle_DoesNotCreateDeck_WhenAllNotesAreAlreadyImported()
    {
        var existingCard = new Card
        {
            Front = "old",
            Back = "old",
            ImportSource = "anki",
            ExternalId = "1",
            Deck = new Deck { CreatedBy = 10, Title = "Existing", TermLanguageCode = "en", DefinitionLanguageCode = "es" },
        };
        _cardRepository.Setup(r => r.GetMany(It.IsAny<Expression<Func<Card, bool>>>()))
            .ReturnsAsync(new List<Card> { existingCard });

        _parser.Setup(p => p.ParseAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AnkiPackage
            {
                Decks =
                {
                    new AnkiDeck
                    {
                        Name = "Deck A",
                        Notes = { new AnkiNote { CardExternalId = "1", Front = "dup", Back = "dup" } },
                    },
                },
            });

        var result = await _sut.Handle(CreateCommand(), CancellationToken.None);

        Assert.False(result.HasErrors);
        Assert.Empty(result.Value!.Decks);
        Assert.Equal(0, result.Value.TotalCardsImported);
        Assert.Equal(1, result.Value.TotalCardsSkipped);
        _deckRepository.Verify(r => r.AddAsync(It.IsAny<Deck>()), Times.Never);
    }

    private static ImportAnkiPackageCommand CreateCommand() => new()
    {
        FileStream = new MemoryStream(),
        TermLanguageCode = "en",
        DefinitionLanguageCode = "es",
        CreatedBy = 10,
    };
}
