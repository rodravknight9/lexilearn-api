using Lexilearn.Application.Contracts.Persistence.Repository;
using Lexilearn.Domain;
using Lexilearn.MySql.Persistence;
using Lexilearn.MySql.Repository.Base;

namespace Lexilearn.MySql.Repository;

public class PracticeSessionRepository : RepositoryBase<PracticeSession>, IPracticeSessionRepository
{
    public PracticeSessionRepository(LexilearnDbContext context) : base(context)
    {            
    }
}