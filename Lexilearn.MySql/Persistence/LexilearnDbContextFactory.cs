using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lexilearn.MySql.Persistence
{
    // Design-time factory so `dotnet ef migrations add` can scaffold migrations
    // without a live MySQL connection (Pomelo's ServerVersion.AutoDetect needs one).
    // Reads ConnectionStrings__LexilearnDb (the same env var the migrator container and
    // webapi use) so `dotnet ef database update` connects to the right host in both the
    // container network (server=mysql) and on the host machine (server=localhost).
    public class LexilearnDbContextFactory : IDesignTimeDbContextFactory<LexilearnDbContext>
    {
        public LexilearnDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__LexilearnDb")
                ?? "server=localhost;Database=LexilearnDb;Uid=root;Pwd=root;";

            var optionsBuilder = new DbContextOptionsBuilder<LexilearnDbContext>();
            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 4, 0)));

            return new LexilearnDbContext(optionsBuilder.Options);
        }
    }
}
