using Lexilearn.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Lexilearn.Domain.Common;

namespace Lexilearn.MySql.Persistence
{
    public class LexilearnDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<PracticeSessionCards> PracticeSessionCards { get; set; }
        public DbSet<PracticeSession> PracticeSessions { get; set; }
        public LexilearnDbContext(DbContextOptions<LexilearnDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
            {
                switch (entry.State)
                {
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Deleted:
                        break;
                    case EntityState.Modified:
                        if (entry.Entity is AuditoryBaseDomain auditoryModified)
                        {
                            auditoryModified.LastModifiedDate = DateTime.UtcNow;
                        }
                        break;
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        if (entry.Entity is AuditoryBaseDomain auditoryCreated)
                        {
                            auditoryCreated.IsActive = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
