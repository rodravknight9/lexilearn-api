using Lexilearn.Application.Contracts.Persistence.Repository;
using Lexilearn.Domain;
using Lexilearn.MySql.Persistence;
using Lexilearn.MySql.Repository.Base;

namespace Lexilearn.MySql.Repository
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(LexilearnDbContext context) : base(context)
        {
        }
    }
}
