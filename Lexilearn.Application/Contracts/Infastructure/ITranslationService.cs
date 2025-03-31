using Lexilearn.Application.Models.LexiLearn;

namespace Lexilearn.Application.Contracts.Infastructure
{
    public interface ITranslationService
    {
        public Task<TranslationResponse> TranslateText(TranslationRequest request);
    }
}
