using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Domain.Common;
using Lexilearn.MySql.Repository.Base;
using System.Collections;

namespace Lexilearn.MySql.Persistence
{
    public partial class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;
        private readonly LexilearnDbContext _context;
        public LexilearnDbContext Context => _context;

        public UnitOfWork(LexilearnDbContext context)
        {
            _context = context;
        }
        public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryBase<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IAsyncRepository<TEntity>)_repositories[type];
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
