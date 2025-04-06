using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.MySql.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lexilearn.MySql
{
    public static class PersistenceServiceRegistrartion
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("LexilearnDb");
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            services.AddDbContext<LexilearnDbContext>(options =>
                options.UseMySql(connectionString, serverVersion)
            );

            //services.AddScoped<IUnitOfWork>
            //services.AddScoped(typeof(IAsyncRepository), typeof(RepositoryBase))

            return services;
        }
    }
}
