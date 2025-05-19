using Lexilearn.DataTransfer.Translation;
using Lexilearn.Web.Models;

namespace Lexilearn.Web.Services.Interfaces
{
    public interface ITranslationService
    {
        public Task<TranslationOutput> Translate(TranslationModel model);
    }
}
