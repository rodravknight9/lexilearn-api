using Lexilearn.AnkiImport.Services;
using Lexilearn.Application.Contracts.Infastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Lexilearn.AnkiImport;

public static class InfrastructureAnkiImportRegistration
{
    public static IServiceCollection AddInfrastructureAnkiImportService(this IServiceCollection services)
    {
        services.AddScoped<IAnkiPackageParser, AnkiPackageParser>();

        return services;
    }
}
