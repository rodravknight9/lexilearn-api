using Lexilearn.Application.Models.AnkiImport;

namespace Lexilearn.Application.Contracts.Infastructure
{
    public interface IAnkiPackageParser
    {
        Task<AnkiPackage> ParseAsync(Stream apkgStream, CancellationToken cancellationToken);
    }
}
