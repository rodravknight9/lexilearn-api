using Lexilearn.Application.Contracts.Infastructure;
using Lexilearn.LibreTranslate.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lexilearn.LibreTranslate;

public static class InfrastructureLibreTranslateRegistration
{
    public static IServiceCollection AddInfrastructureLibreTranslateService(this IServiceCollection services)
    {
        services.AddHttpClient<ITranslationService, TranslationService>();
        return services;
    }
}
