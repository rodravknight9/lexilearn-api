using Lexilearn.Application.Contracts.Persistence.Repository;
using Lexilearn.Domain.Common;

namespace Lexilearn.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IDeckRepository DeckRepository { get; }
        ICardRepository CardRepository { get; }
        IPracticeSessionRepository PracticeSessionRepository { get; }
        IPracticeSessionCardsRepository PracticeSessionCardsRepository { get; }
        
        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel;
        Task<int> Complete();
    }
}
