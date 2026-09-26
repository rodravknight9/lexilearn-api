using Lexilearn.Application.Models.CustomTranslate;
using Lexilearn.Domain.Enums;

namespace Lexilearn.Application.Features.Translation.Profiles.Common;

public class TranslationProfileResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public TranslationProfileKind Kind { get; set; }
    public bool IsDefault { get; set; }
    public string Url { get; set; } = null!;
    public string HttpMethod { get; set; } = null!;
    public List<RequestEntry> Headers { get; set; } = [];
    public List<RequestEntry> Body { get; set; } = [];
    public string ResponsePath { get; set; } = null!;
}
