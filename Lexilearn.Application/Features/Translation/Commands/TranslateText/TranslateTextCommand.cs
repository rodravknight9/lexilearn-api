using System.Text.Json.Serialization;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Application.Models.LibreTranslate;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Commands.TranslateText
{
    public class TranslateTextCommand : IRequest<Result<TranslationResponse>>
    {
        public string Text { get; set; } = null!;
        public string LanguageSourceCode { get; set; } = null!;
        public string LanguageTargetCode { get; set; } = null!;
        public int? TranslationProfileId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
