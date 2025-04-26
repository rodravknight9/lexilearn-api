using Lexilearn.Application.Models.LibreTranslate;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Commands.TranslateText
{
    public class TranslateTextCommand : IRequest<TranslationResponse>
    {
        public string Text { get; set; } = null!;
        public string LanguageSourceCode { get; set; } = null!;
        public string LanguageTargetCode { get; set; } = null!;
    }
}
