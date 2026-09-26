using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Lexilearn.Application.Models.CustomTranslate;
using Lexilearn.Application.Models.LibreTranslate;
using Lexilearn.CustomTranslate.Services;
using Lexilearn.LibreTranslate.Services;

namespace Lexilearn.Application.Tests.LibreTranslate;

public class TranslationServiceTests
{
    [Fact]
    public async Task TranslateText_PostsToTheProfileBaseUrl()
    {
        HttpRequestMessage? captured = null;
        var handler = new StubHandler(request =>
        {
            captured = request;
            return JsonResponse("hola");
        });
        using var client = new HttpClient(handler);
        var sut = new TranslationService(client);

        var result = await sut.TranslateText(
            new TranslationRequest { q = "hi", source = "en", target = "es" },
            "http://localhost:5000");

        Assert.Equal("hola", result.translatedText);
        Assert.Equal("http://localhost:5000/translate", captured!.RequestUri!.ToString());
    }

    [Fact]
    public async Task TranslateText_ReturnsUpstreamErrorMessage()
    {
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("""{"error":"es is not supported"}""")
        });
        using var client = new HttpClient(handler);
        var sut = new TranslationService(client);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            sut.TranslateText(
                new TranslationRequest { q = "hi", source = "en", target = "es" },
                "http://localhost:5000"));

        Assert.Equal("es is not supported", exception.Message);
    }

    [Fact]
    public async Task CustomTranslationService_SendsEditableHeadersAndBody()
    {
        string? body = null;
        string? mediaType = null;
        HttpRequestMessage? captured = null;
        var handler = new StubHandler(async request =>
        {
            captured = request;
            body = request.Content is null ? null : await request.Content.ReadAsStringAsync();
            mediaType = request.Content?.Headers.ContentType?.MediaType;
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"trans":"xin chao"}""")
            };
        });
        using var client = new HttpClient(handler);
        var sut = new CustomTranslationService(client);

        var result = await sut.TranslateAsync(new CustomTranslationCall
        {
            Url = "https://google-translate113.p.rapidapi.com/api/v1/translator/html",
            HttpMethod = "POST",
            Headers =
            [
                new RequestEntry { Key = "x-rapidapi-key", Value = "secret" },
                new RequestEntry { Key = "x-rapidapi-host", Value = "google-translate113.p.rapidapi.com" }
            ],
            Body =
            [
                new RequestEntry { Key = "from", Value = "auto" },
                new RequestEntry { Key = "to", Value = "{{target}}" },
                new RequestEntry { Key = "html", Value = "{{text}}" }
            ],
            ResponsePath = "trans",
            Text = "Hello",
            Source = "en",
            Target = "vi"
        });

        Assert.Equal("xin chao", result.translatedText);
        Assert.Equal(HttpMethod.Post, captured!.Method);
        Assert.Equal("secret", captured.Headers.GetValues("x-rapidapi-key").Single());
        Assert.Equal("google-translate113.p.rapidapi.com", captured.Headers.GetValues("x-rapidapi-host").Single());

        using var document = JsonDocument.Parse(body!);
        Assert.Equal("auto", document.RootElement.GetProperty("from").GetString());
        Assert.Equal("vi", document.RootElement.GetProperty("to").GetString());
        Assert.Equal("Hello", document.RootElement.GetProperty("html").GetString());
        Assert.Equal("application/json", mediaType);
    }

    private static HttpResponseMessage JsonResponse(string translatedText) =>
        new(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new TranslationResponse { translatedText = translatedText })
        };

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _response;

        public StubHandler(Func<HttpRequestMessage, HttpResponseMessage> response)
        {
            _response = request => Task.FromResult(response(request));
        }

        public StubHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            _response(request);
    }
}
