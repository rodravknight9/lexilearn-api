using Lexilearn.Application.Contracts.Infastructure;
using Lexilearn.CustomTranslate.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lexilearn.CustomTranslate;

public static class InfrastructureCustomTranslateRegistration
{
    public static IServiceCollection AddInfrastructureCustomTranslateService(this IServiceCollection services)
    {
        services.AddHttpClient<ICustomTranslationService, CustomTranslationService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return services;
    }
}
