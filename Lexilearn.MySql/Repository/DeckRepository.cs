using Lexilearn.Application.Contracts.Persistence.Repository;
using Lexilearn.Domain;
using Lexilearn.MySql.Persistence;
using Lexilearn.MySql.Repository.Base;

namespace Lexilearn.MySql.Repository
{
    public class DeckRepository : RepositoryBase<Deck>, IDeckRepository
    {
        public DeckRepository(LexilearnDbContext context) : base(context)
        {            
        }
    }
}
