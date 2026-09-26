using Lexilearn.Application.Models.CustomTranslate;
using Lexilearn.Domain;

namespace Lexilearn.Application.Features.Translation.Profiles.Common;

public static class TranslationProfileMapper
{
    public static TranslationProfileResponse ToResponse(TranslationProfile profile) => new()
    {
        Id = profile.Id,
        Name = profile.Name,
        Kind = profile.Kind,
        IsDefault = profile.IsDefault,
        Url = profile.Url,
        HttpMethod = profile.HttpMethod,
        Headers = RequestEntryJson.Deserialize(profile.HeadersJson),
        Body = RequestEntryJson.Deserialize(profile.BodyJson),
        ResponsePath = profile.ResponsePath
    };

    public static void Apply(TranslationProfile profile, ValidatedTranslationProfile validated, bool isDefault)
    {
        profile.Name = validated.Name;
        profile.Url = validated.Url;
        profile.HttpMethod = validated.HttpMethod;
        profile.HeadersJson = RequestEntryJson.Serialize(validated.Headers);
        profile.BodyJson = RequestEntryJson.Serialize(validated.Body);
        profile.ResponsePath = validated.ResponsePath;
        profile.IsDefault = isDefault;
    }
}
