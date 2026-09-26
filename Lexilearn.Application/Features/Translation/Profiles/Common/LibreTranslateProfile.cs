using Lexilearn.Application.Models.CustomTranslate;
using Lexilearn.Domain;
using Lexilearn.Domain.Enums;

namespace Lexilearn.Application.Features.Translation.Profiles.Common;

public static class LibreTranslateProfile
{
    public const string Name = "LibreTranslate";

    public static string? ValidateUrl(string? url)
    {
        var trimmed = url?.Trim() ?? "";
        if (trimmed.Length is 0 or > 2000 || !TranslationUrl.IsAbsoluteHttpUrl(trimmed))
            return "Url must be an absolute http or https URL.";

        return null;
    }

    public static void Apply(TranslationProfile profile, string url, bool isDefault)
    {
        profile.Kind = TranslationProfileKind.LibreTranslate;
        profile.Name = Name;
        profile.Url = url.Trim();
        profile.HttpMethod = "POST";
        profile.HeadersJson = "[]";
        profile.BodyJson = "[]";
        profile.ResponsePath = "";
        profile.IsDefault = isDefault;
    }
}
