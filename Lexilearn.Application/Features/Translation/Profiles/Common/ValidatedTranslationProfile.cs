using Lexilearn.Application.Models.CustomTranslate;

namespace Lexilearn.Application.Features.Translation.Profiles.Common;

public sealed class ValidatedTranslationProfile
{
    public string? Error { get; init; }
    public string Name { get; init; } = "";
    public string Url { get; init; } = "";
    public string HttpMethod { get; init; } = "POST";
    public IReadOnlyList<RequestEntry> Headers { get; init; } = [];
    public IReadOnlyList<RequestEntry> Body { get; init; } = [];
    public string ResponsePath { get; init; } = "";

    public bool IsValid => Error is null;

    private static readonly HashSet<string> AllowedMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        "GET", "POST", "PUT", "PATCH", "DELETE"
    };

    public static ValidatedTranslationProfile Validate(
        string? name,
        string? url,
        string? httpMethod,
        IReadOnlyList<RequestEntry>? headers,
        IReadOnlyList<RequestEntry>? body,
        string? responsePath)
    {
        var trimmedName = name?.Trim() ?? "";
        if (trimmedName.Length is 0 or > 120)
            return Invalid("Name is required and must be at most 120 characters.");

        var trimmedUrl = url?.Trim() ?? "";
        if (trimmedUrl.Length is 0 or > 2000 || !TranslationUrl.IsAbsoluteHttpUrl(TranslationUrl.Probe(trimmedUrl)))
            return Invalid("Url must be an absolute http or https URL.");

        var method = string.IsNullOrWhiteSpace(httpMethod) ? "POST" : httpMethod.Trim().ToUpperInvariant();
        if (!AllowedMethods.Contains(method))
            return Invalid("HttpMethod must be GET, POST, PUT, PATCH, or DELETE.");

        var trimmedPath = responsePath?.Trim() ?? "";
        if (trimmedPath.Length is 0 or > 300)
            return Invalid("Response path is required and must be at most 300 characters.");

        if (!TryNormalize(headers, caseInsensitiveKeys: true, out var normalizedHeaders, out var headerError))
            return Invalid(headerError!);

        if (!TryNormalize(body, caseInsensitiveKeys: false, out var normalizedBody, out var bodyError))
            return Invalid(bodyError!);

        return new ValidatedTranslationProfile
        {
            Name = trimmedName,
            Url = trimmedUrl,
            HttpMethod = method,
            Headers = normalizedHeaders,
            Body = normalizedBody,
            ResponsePath = trimmedPath
        };
    }

    private static bool TryNormalize(
        IReadOnlyList<RequestEntry>? entries,
        bool caseInsensitiveKeys,
        out List<RequestEntry> normalized,
        out string? error)
    {
        normalized = [];
        error = null;
        var comparer = caseInsensitiveKeys ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        var seen = new HashSet<string>(comparer);
        var label = caseInsensitiveKeys ? "Header" : "Body";

        foreach (var entry in entries ?? [])
        {
            var key = entry.Key?.Trim() ?? "";
            if (key.Length is 0 or > 256)
            {
                error = $"{label} entries need a key of at most 256 characters.";
                return false;
            }

            var value = entry.Value ?? "";
            if (value.Length > 8000)
            {
                error = $"{label} values must be at most 8000 characters.";
                return false;
            }

            if (!seen.Add(key))
            {
                error = $"{label} keys must be unique.";
                return false;
            }

            normalized.Add(new RequestEntry { Key = key, Value = value });
        }

        return true;
    }

    private static ValidatedTranslationProfile Invalid(string error) => new() { Error = error };
}
