using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lexilearn.MySql.Persistence
{
    // Design-time factory so `dotnet ef migrations add` can scaffold migrations
    // without a live MySQL connection (Pomelo's ServerVersion.AutoDetect needs one).
    public class LexilearnDbContextFactory : IDesignTimeDbContextFactory<LexilearnDbContext>
    {
        public LexilearnDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LexilearnDbContext>();
            optionsBuilder.UseMySql(
                "server=localhost;Database=LexilearnDb;Uid=root;Pwd=root;",
                new MySqlServerVersion(new Version(8, 4, 0)));

            return new LexilearnDbContext(optionsBuilder.Options);
        }
    }
}
