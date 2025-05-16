using Lexilearn.Application.Contracts.Persistence.Repository;
using Lexilearn.Domain;
using Lexilearn.MySql.Persistence;
using Lexilearn.MySql.Repository.Base;

namespace Lexilearn.MySql.Repository;

public class PracticeSessionCardsRepository : RepositoryBase<PracticeSessionCards>,IPracticeSessionCardsRepository
{
    public PracticeSessionCardsRepository(LexilearnDbContext context) : base(context)
    {            
    }
}