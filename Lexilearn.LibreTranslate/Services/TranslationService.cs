using System.Net.Http.Json;
using Lexilearn.Application.Contracts.Infastructure;
using Lexilearn.Application.Models.LibreTranslate;
using Lexilearn.Application.Translation;

namespace Lexilearn.LibreTranslate.Services;

public class TranslationService : ITranslationService
{
    private readonly HttpClient _httpClient;

    public TranslationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<TranslationResponse> TranslateText(TranslationRequest request, string baseUrl)
    {
        var endpoint = new Uri(new Uri($"{baseUrl.TrimEnd('/')}/"), "translate");

        var response = await _httpClient.PostAsJsonAsync(endpoint, request);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(await TranslationHttpError.ReadAsync(response));

        var translation = await response.Content.ReadFromJsonAsync<TranslationResponse>();
        if (translation is null)
            throw new InvalidOperationException("LibreTranslate returned an empty response.");

        return translation;
    }
}
