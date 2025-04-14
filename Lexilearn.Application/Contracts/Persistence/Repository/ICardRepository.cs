using Lexilearn.Domain;
using Lexilearn.Shared;

namespace Lexilearn.Application.Contracts.Persistence.Repository
{
    public interface ICardRepository : IAsyncRepository<Card>
    {
        public Task<IReadOnlyList<Card>> GetByDeckId(PaginationSettings pagination, int deckId);
    }
}
