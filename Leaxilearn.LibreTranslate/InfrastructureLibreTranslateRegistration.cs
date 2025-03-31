using Leaxilearn.LibreTranslate.Models;
using Leaxilearn.LibreTranslate.Services;
using Lexilearn.Application.Contracts.Infastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Leaxilearn.LibreTranslate
{
    public static class InfrastructureLibreTranslateRegistration
    {
        public static IServiceCollection AddInfrastructureLibreTranslateService(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ITranslationService, TranslationService>();

            services.Configure<LibreTranslateSettings>(
                configuration.GetSection("LibreTranslateSettings"));
            
            return services;
        }
    }
}
