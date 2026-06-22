using Lexilearn.Domain;

namespace Lexilearn.Application.Contracts.Services;

public interface IDeckOwnershipService
{
    Task<Deck?> GetOwnedDeckAsync(int deckId, int userId, CancellationToken cancellationToken = default);
    Task<Card?> GetOwnedCardAsync(int cardId, int userId, CancellationToken cancellationToken = default);
}
