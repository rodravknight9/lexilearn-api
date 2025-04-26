using Lexilearn.Application.Models.LibreTranslate;

namespace Lexilearn.Application.Contracts.Infastructure
{
    public interface ITranslationService
    {
        public Task<TranslationResponse> TranslateText(TranslationRequest request);
    }
}
