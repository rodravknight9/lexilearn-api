 using Lexilearn.Application.Contracts.Persistence.Repository;
using Lexilearn.Domain;
using Lexilearn.MySql.Persistence;
using Lexilearn.MySql.Repository.Base;
using Lexilearn.Shared;

namespace Lexilearn.MySql.Repository
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(LexilearnDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Card>> GetByDeckId(PaginationSettings pagination, int deckId)
        {
            return await GetAsync(pagination, c => c.DeckId == deckId);
        }
    }
}
