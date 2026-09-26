using Lexilearn.Domain.Common;
using Lexilearn.Domain.Enums;

namespace Lexilearn.Domain;

public class TranslationProfile : AuditoryBaseDomain
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public TranslationProfileKind Kind { get; set; } = TranslationProfileKind.Custom;
    public bool IsDefault { get; set; }
    public string Url { get; set; } = null!;
    public string HttpMethod { get; set; } = "POST";
    public string HeadersJson { get; set; } = "[]";
    public string BodyJson { get; set; } = "[]";
    public string ResponsePath { get; set; } = null!;
}
