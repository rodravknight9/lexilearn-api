using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Domain;

namespace Lexilearn.Application.Services;

public class DeckOwnershipService : IDeckOwnershipService
{
    private readonly IUnitOfWork _unitOfWork;

    public DeckOwnershipService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Deck?> GetOwnedDeckAsync(int deckId, int userId, CancellationToken cancellationToken = default)
    {
        var deck = await _unitOfWork.DeckRepository.GetByIdAsync(deckId);
        if (deck is null || deck.CreatedBy != userId || !deck.IsActive)
            return null;

        return deck;
    }

    public async Task<Card?> GetOwnedCardAsync(int cardId, int userId, CancellationToken cancellationToken = default)
    {
        var card = await _unitOfWork.CardRepository.GetByIdAsync(cardId);
        if (card is null || !card.IsActive)
            return null;

        var deck = await _unitOfWork.DeckRepository.GetByIdAsync(card.DeckId);
        if (deck is null || deck.CreatedBy != userId || !deck.IsActive)
            return null;

        return card;
    }
}
