using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Persistence.Repository;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Features.Lexilearn.PracticeSession.Commands.SavePracticeSession;
using Lexilearn.Domain;
using Lexilearn.Domain.Enums;
using Moq;

namespace Lexilearn.Application.Tests.Features.PracticeSession;

public class SavePracticeSessionCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IPracticeSessionRepository> _practiceSessionRepository = new();
    private readonly Mock<IAsyncRepository<CardSchedulingState>> _schedulingStateRepository = new();
    private readonly Mock<IDeckOwnershipService> _ownership = new();
    private readonly Mock<ISpacedRepetitionScheduler> _scheduler = new();
    private readonly SavePracticeSessionCommandHandler _sut;

    public SavePracticeSessionCommandHandlerTests()
    {
        _unitOfWork.Setup(u => u.PracticeSessionRepository).Returns(_practiceSessionRepository.Object);
        _unitOfWork.Setup(u => u.Repository<CardSchedulingState>()).Returns(_schedulingStateRepository.Object);

        _ownership.Setup(o => o.GetOwnedDeckAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Deck { Id = 1, CreatedBy = 10, Title = "Deck", TermLanguageCode = "en", DefinitionLanguageCode = "es" });
        _ownership.Setup(o => o.GetOwnedCardAsync(5, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Card { Id = 5, DeckId = 1, Front = "f", Back = "b", Deck = null! });
        _schedulingStateRepository.Setup(r => r.GetOne(It.IsAny<System.Linq.Expressions.Expression<Func<CardSchedulingState, bool>>>()))
            .ReturnsAsync(new CardSchedulingState { CardId = 5 });

        _sut = new SavePracticeSessionCommandHandler(_unitOfWork.Object, _ownership.Object, _scheduler.Object);
    }

    [Fact]
    public async Task Handle_StampsCreatedByOnTheSession_SoSessionHistoryCanBeQueriedByUser()
    {
        Domain.PracticeSession? addedSession = null;
        _practiceSessionRepository.Setup(r => r.AddAsync(It.IsAny<Domain.PracticeSession>()))
            .Callback<Domain.PracticeSession>(s => addedSession = s)
            .ReturnsAsync((Domain.PracticeSession s) => s);

        var command = new SavePracticeSessionCommand
        {
            DeckId = 1,
            CreatedBy = 10,
            Cards = [new CardInPractice { CardId = 5, Rating = ReviewRating.Easy }],
        };

        var result = await _sut.Handle(command, CancellationToken.None);

        Assert.False(result.HasErrors);
        Assert.NotNull(addedSession);
        Assert.Equal(10, addedSession!.CreatedBy);
    }
}
