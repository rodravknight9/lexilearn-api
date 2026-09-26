using System.Text.Json.Serialization;

namespace Lexilearn.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TranslationProfileKind
{
    Custom = 0,
    LibreTranslate = 1
}
