using System.Text.Json;

namespace Lexilearn.Application.Models.CustomTranslate;

public static class RequestEntryJson
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public static string Serialize(IEnumerable<RequestEntry> entries) =>
        JsonSerializer.Serialize(entries, Options);

    public static List<RequestEntry> Deserialize(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return [];

        return JsonSerializer.Deserialize<List<RequestEntry>>(json, Options) ?? [];
    }
}
