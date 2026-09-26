using Lexilearn.Application.Models.CustomTranslate;
using Lexilearn.Application.Models.LibreTranslate;

namespace Lexilearn.Application.Contracts.Infastructure;

public interface ICustomTranslationService
{
    Task<TranslationResponse> TranslateAsync(CustomTranslationCall call, CancellationToken cancellationToken = default);
}
