using Lexilearn.Application.Contracts.Infastructure;
using Lexilearn.LibreTranslate.Models;
using Lexilearn.LibreTranslate.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lexilearn.LibreTranslate;

public static class InfrastructureLibreTranslateRegistration
{
    public static IServiceCollection AddInfrastructureLibreTranslateService(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LibreTranslateSettings>(
            configuration.GetSection("LibreTranslateSettings"));

        services.AddHttpClient<ITranslationService, TranslationService>();

        return services;
    }
}
