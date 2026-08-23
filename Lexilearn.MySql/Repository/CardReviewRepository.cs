using Lexilearn.Application.Contracts.Persistence.Repository;
using Lexilearn.Domain;
using Lexilearn.MySql.Persistence;
using Lexilearn.MySql.Repository.Base;

namespace Lexilearn.MySql.Repository;

public class CardReviewRepository : RepositoryBase<CardReview>, ICardReviewRepository
{
    public CardReviewRepository(LexilearnDbContext context) : base(context)
    {
    }
}
