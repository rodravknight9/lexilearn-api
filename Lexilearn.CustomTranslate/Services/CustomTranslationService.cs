using System.Net.Http.Headers;
using System.Text;
using Lexilearn.Application.Contracts.Infastructure;
using Lexilearn.Application.Models.CustomTranslate;
using Lexilearn.Application.Models.LibreTranslate;
using Lexilearn.Application.Translation;

namespace Lexilearn.CustomTranslate.Services;

public class CustomTranslationService : ICustomTranslationService
{
    private static readonly HashSet<string> AllowedMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        "GET", "POST", "PUT", "PATCH", "DELETE"
    };

    private readonly HttpClient _httpClient;

    public CustomTranslationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TranslationResponse> TranslateAsync(CustomTranslationCall call, CancellationToken cancellationToken = default)
    {
        if (!AllowedMethods.Contains(call.HttpMethod))
            throw new InvalidOperationException("HttpMethod must be GET, POST, PUT, PATCH, or DELETE.");

        var url = CustomRequestBuilder.Apply(call.Url, call.Text, call.Source, call.Target);
        if (!TranslationUrl.IsAbsoluteHttpUrl(url))
            throw new InvalidOperationException("Translation URL must be an absolute http or https URL.");

        using var request = new HttpRequestMessage(new HttpMethod(call.HttpMethod), url);
        var headers = CustomRequestBuilder.ApplyHeaders(call.Headers, call.Text, call.Source, call.Target);
        string? contentType = null;

        foreach (var header in headers)
        {
            if (header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
            {
                contentType = header.Value;
                continue;
            }

            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (call.Body.Count > 0)
        {
            var bodyJson = CustomRequestBuilder.BuildBodyJson(call.Body, call.Text, call.Source, call.Target);
            request.Content = new StringContent(bodyJson, Encoding.UTF8);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.TryParse(contentType, out var parsed)
                ? parsed
                : new MediaTypeHeaderValue("application/json");
        }

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(await TranslationHttpError.ReadAsync(response));

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        return new TranslationResponse
        {
            translatedText = CustomRequestBuilder.ReadResponsePath(payload, call.ResponsePath)
        };
    }
}
