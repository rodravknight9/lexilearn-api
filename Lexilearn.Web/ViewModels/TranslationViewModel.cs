using Lexilearn.DataTransfer.Translation;
using Lexilearn.Web.Models;
using Lexilearn.Web.Services.Interfaces;

namespace Lexilearn.Web.ViewModels
{
    public class TranslationViewModel
    {
        public TranslationModel Model { get; set; } = new TranslationModel();
        public TranslationOutput TranslationOutput { get; set; } = new TranslationOutput();
        private readonly ITranslationService _translationService;
        public TranslationViewModel(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        public async Task Send(TranslationModel model)
        {
            TranslationOutput = await _translationService.Translate(model);
        }

    }
}
