using Lexilearn.Application.Contracts.Infastructure;
using Lexilearn.Application.Models.LibreTranslate;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Commands.TranslateText
{
    public class TranslationHandler : IRequestHandler<TranslateTextCommand, TranslationResponse>
    {
        private readonly ITranslationService _translationService;
        public TranslationHandler(ITranslationService translationService)
        {
            _translationService = translationService;
        }
        public Task<TranslationResponse> Handle(TranslateTextCommand request, CancellationToken cancellationToken)
        {
            //TODO: add automapper here
            var requestLibreTranslate = new TranslationRequest
            {
                q = request.Text,
                source = request.LanguageSourceCode,
                target = request.LanguageTargetCode
            };
            return _translationService.TranslateText(requestLibreTranslate);
        }
    }
}
