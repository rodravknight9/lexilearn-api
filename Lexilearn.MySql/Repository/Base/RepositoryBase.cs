using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Domain.Common;
using Lexilearn.MySql.Persistence;
using Lexilearn.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace Lexilearn.MySql.Repository.Base
{
    public class RepositoryBase<TEntity> : IAsyncRepository<TEntity> where TEntity : BaseDomainModel
    {
        private readonly LexilearnDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public RepositoryBase(LexilearnDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IReadOnlyList<TEntity>> GetMany(PaginationSettings pagination)
        {
            int pageNumber = pagination.PageNumber ?? 0;
            int pageSize = pagination.PageSize ?? 10;
            return await _dbSet
                    .Skip(pageNumber * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
