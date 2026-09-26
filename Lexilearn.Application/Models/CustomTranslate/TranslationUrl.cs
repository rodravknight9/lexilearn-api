namespace Lexilearn.Application.Models.CustomTranslate;

public static class TranslationUrl
{
    public const string TextPlaceholder = "{{text}}";
    public const string SourcePlaceholder = "{{source}}";
    public const string TargetPlaceholder = "{{target}}";

    public static string Probe(string value) =>
        (value ?? "")
            .Replace(TextPlaceholder, "text", StringComparison.Ordinal)
            .Replace(SourcePlaceholder, "en", StringComparison.Ordinal)
            .Replace(TargetPlaceholder, "es", StringComparison.Ordinal);

    public static bool IsAbsoluteHttpUrl(string url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
}
