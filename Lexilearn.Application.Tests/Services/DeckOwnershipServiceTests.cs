using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Persistence.Repository;
using Lexilearn.Application.Services;
using Lexilearn.Domain;
using Moq;

namespace Lexilearn.Application.Tests.Services;

public class DeckOwnershipServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IDeckRepository> _deckRepository = new();
    private readonly Mock<ICardRepository> _cardRepository = new();
    private readonly DeckOwnershipService _sut;

    public DeckOwnershipServiceTests()
    {
        _unitOfWork.Setup(u => u.DeckRepository).Returns(_deckRepository.Object);
        _unitOfWork.Setup(u => u.CardRepository).Returns(_cardRepository.Object);
        _sut = new DeckOwnershipService(_unitOfWork.Object);
    }

    [Fact]
    public async Task GetOwnedDeckAsync_ReturnsNull_WhenDeckNotFound()
    {
        _deckRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Deck)null!);

        var result = await _sut.GetOwnedDeckAsync(1, userId: 10);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetOwnedDeckAsync_ReturnsNull_WhenUserDoesNotOwnDeck()
    {
        var deck = CreateDeck(id: 1, createdBy: 99, isActive: true);
        _deckRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(deck);

        var result = await _sut.GetOwnedDeckAsync(1, userId: 10);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetOwnedDeckAsync_ReturnsNull_WhenDeckIsInactive()
    {
        var deck = CreateDeck(id: 1, createdBy: 10, isActive: false);
        _deckRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(deck);

        var result = await _sut.GetOwnedDeckAsync(1, userId: 10);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetOwnedDeckAsync_ReturnsDeck_WhenOwnedAndActive()
    {
        var deck = CreateDeck(id: 1, createdBy: 10, isActive: true);
        _deckRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(deck);

        var result = await _sut.GetOwnedDeckAsync(1, userId: 10);

        Assert.Same(deck, result);
    }

    [Fact]
    public async Task GetOwnedCardAsync_ReturnsNull_WhenCardNotFound()
    {
        _cardRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Card)null!);

        var result = await _sut.GetOwnedCardAsync(1, userId: 10);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetOwnedCardAsync_ReturnsNull_WhenCardIsInactive()
    {
        var card = CreateCard(id: 1, deckId: 5, isActive: false);
        _cardRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(card);

        var result = await _sut.GetOwnedCardAsync(1, userId: 10);

        Assert.Null(result);
        _deckRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetOwnedCardAsync_ReturnsNull_WhenDeckNotFound()
    {
        var card = CreateCard(id: 1, deckId: 5, isActive: true);
        _cardRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(card);
        _deckRepository.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((Deck)null!);

        var result = await _sut.GetOwnedCardAsync(1, userId: 10);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetOwnedCardAsync_ReturnsNull_WhenUserDoesNotOwnDeck()
    {
        var card = CreateCard(id: 1, deckId: 5, isActive: true);
        var deck = CreateDeck(id: 5, createdBy: 99, isActive: true);
        _cardRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(card);
        _deckRepository.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(deck);

        var result = await _sut.GetOwnedCardAsync(1, userId: 10);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetOwnedCardAsync_ReturnsNull_WhenDeckIsInactive()
    {
        var card = CreateCard(id: 1, deckId: 5, isActive: true);
        var deck = CreateDeck(id: 5, createdBy: 10, isActive: false);
        _cardRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(card);
        _deckRepository.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(deck);

        var result = await _sut.GetOwnedCardAsync(1, userId: 10);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetOwnedCardAsync_ReturnsCard_WhenOwnedAndActive()
    {
        var card = CreateCard(id: 1, deckId: 5, isActive: true);
        var deck = CreateDeck(id: 5, createdBy: 10, isActive: true);
        _cardRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(card);
        _deckRepository.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(deck);

        var result = await _sut.GetOwnedCardAsync(1, userId: 10);

        Assert.Same(card, result);
    }

    private static Deck CreateDeck(int id, int createdBy, bool isActive) => new()
    {
        Id = id,
        CreatedBy = createdBy,
        IsActive = isActive,
        Title = "Test Deck",
        TermLanguageCode = "en",
        DefinitionLanguageCode = "pt"
    };

    private static Card CreateCard(int id, int deckId, bool isActive) => new()
    {
        Id = id,
        DeckId = deckId,
        IsActive = isActive,
        Front = "hello",
        Back = "olá",
        Deck = null!
    };
}
