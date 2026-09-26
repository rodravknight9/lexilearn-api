using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Lexilearn.Application.Models.CustomTranslate;

namespace Lexilearn.CustomTranslate.Services;

public static class CustomRequestBuilder
{
    private static readonly Regex Placeholder = new(
        @"\{\{(text|source|target)\}\}",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public static string Apply(string template, string text, string source, string target)
    {
        return Placeholder.Replace(template ?? "", match => match.Groups[1].Value switch
        {
            "text" => text,
            "source" => source,
            "target" => target,
            _ => match.Value
        });
    }

    public static string BuildBodyJson(
        IReadOnlyList<RequestEntry> entries,
        string text,
        string source,
        string target)
    {
        var body = new JsonObject();
        foreach (var entry in entries)
        {
            body[entry.Key] = Apply(entry.Value, text, source, target);
        }

        return body.ToJsonString(new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }

    public static IReadOnlyList<RequestEntry> ApplyHeaders(
        IReadOnlyList<RequestEntry> headers,
        string text,
        string source,
        string target)
    {
        return headers
            .Select(header => new RequestEntry
            {
                Key = header.Key,
                Value = Apply(header.Value, text, source, target)
            })
            .ToList();
    }

    public static string ReadResponsePath(string json, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new InvalidOperationException("Response path is required.");

        JsonDocument document;
        try
        {
            document = JsonDocument.Parse(json);
        }
        catch (JsonException)
        {
            throw new InvalidOperationException("Translation response was not valid JSON.");
        }

        using (document)
        {
            var current = document.RootElement;
            foreach (var segment in path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (current.ValueKind == JsonValueKind.Array)
                {
                    if (!int.TryParse(segment, out var index) || index < 0 || index >= current.GetArrayLength())
                        throw Missing(path);

                    current = current[index];
                    continue;
                }

                if (current.ValueKind != JsonValueKind.Object || !current.TryGetProperty(segment, out var next))
                    throw Missing(path);

                current = next;
            }

            if (current.ValueKind != JsonValueKind.String)
                throw new InvalidOperationException($"Translation response path '{path}' is not a string.");

            return current.GetString()
                ?? throw new InvalidOperationException($"Translation response path '{path}' is empty.");
        }
    }

    private static InvalidOperationException Missing(string path) =>
        new($"Translation response does not contain '{path}'.");
}
