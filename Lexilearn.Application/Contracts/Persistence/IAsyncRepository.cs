using Lexilearn.Domain.Common;
using Lexilearn.Shared;
using System.Linq.Expressions;

namespace Lexilearn.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : BaseDomainModel
    {
        Task<IReadOnlyList<T>> GetMany(PaginationSettings pagination);
        Task<IReadOnlyList<T>> GetMany(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(PaginationSettings pagination, Expression<Func<T, bool>> predicate);
        Task<T> GetOne(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
