using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Services;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Lexilearn.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddMapster();
            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
            services.AddScoped<IDeckOwnershipService, DeckOwnershipService>();
            return services;
        }
    }
}
