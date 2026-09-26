using System.Text.Json;
using Lexilearn.Application.Models.CustomTranslate;
using Lexilearn.CustomTranslate.Services;

namespace Lexilearn.Application.Tests.CustomTranslate;

public class CustomRequestBuilderTests
{
    [Fact]
    public void BuildBodyJson_UsesCallerKeysLiteralsAndPlaceholders()
    {
        var json = CustomRequestBuilder.BuildBodyJson(
            [
                new RequestEntry { Key = "from", Value = "auto" },
                new RequestEntry { Key = "to", Value = "{{target}}" },
                new RequestEntry { Key = "html", Value = "Hello {{text}}" }
            ],
            text: "world",
            source: "en",
            target: "vi");

        using var document = JsonDocument.Parse(json);
        Assert.Equal("auto", document.RootElement.GetProperty("from").GetString());
        Assert.Equal("vi", document.RootElement.GetProperty("to").GetString());
        Assert.Equal("Hello world", document.RootElement.GetProperty("html").GetString());
        Assert.False(document.RootElement.TryGetProperty("q", out _));
    }

    [Fact]
    public void BuildBodyJson_JsonEscapesInsertedTextOnce()
    {
        var json = CustomRequestBuilder.BuildBodyJson(
            [new RequestEntry { Key = "html", Value = "{{text}}" }],
            text: "say \"hi\" <b>",
            source: "en",
            target: "vi");

        using var document = JsonDocument.Parse(json);
        Assert.Equal("say \"hi\" <b>", document.RootElement.GetProperty("html").GetString());
    }

    [Fact]
    public void Apply_DoesNotExpandPlaceholdersInsideInsertedText()
    {
        var value = CustomRequestBuilder.Apply("{{text}}", "keep {{source}}", "en", "vi");

        Assert.Equal("keep {{source}}", value);
    }

    [Fact]
    public void ApplyHeaders_KeepsCallerKeysAndFillsValues()
    {
        var headers = CustomRequestBuilder.ApplyHeaders(
            [
                new RequestEntry { Key = "x-rapidapi-key", Value = "secret" },
                new RequestEntry { Key = "x-rapidapi-host", Value = "{{target}}.example" }
            ],
            text: "hello",
            source: "en",
            target: "vi");

        Assert.Equal("x-rapidapi-key", headers[0].Key);
        Assert.Equal("secret", headers[0].Value);
        Assert.Equal("x-rapidapi-host", headers[1].Key);
        Assert.Equal("vi.example", headers[1].Value);
    }

    [Fact]
    public void ReadResponsePath_ReadsNestedString()
    {
        const string json = """
            {
              "trans": "xin chao",
              "data": { "translations": [ { "text": "nested" } ] }
            }
            """;

        Assert.Equal("xin chao", CustomRequestBuilder.ReadResponsePath(json, "trans"));
        Assert.Equal("nested", CustomRequestBuilder.ReadResponsePath(json, "data.translations.0.text"));
    }

    [Fact]
    public void ReadResponsePath_ThrowsWhenPathIsMissing()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            CustomRequestBuilder.ReadResponsePath("""{"trans":"ok"}""", "missing"));

        Assert.Contains("missing", exception.Message);
    }
}
