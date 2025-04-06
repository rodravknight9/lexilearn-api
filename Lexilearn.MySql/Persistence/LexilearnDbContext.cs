using Lexilearn.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Lexilearn.MySql.Persistence
{
    public class LexilearnDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public LexilearnDbContext(DbContextOptions<LexilearnDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var connectionString = "server=localhost;Database=LexilearnDb;Uid=root;Pwd=root;";
            //var serverVersion = ServerVersion.AutoDetect(connectionString);
            //optionsBuilder.UseMySql(connectionString, serverVersion);
            //services.AddDbContext<LexilearnDbContext>(options =>
            //);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
