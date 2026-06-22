using Lexilearn.Application.Contracts.Infastructure;
using Lexilearn.Application.Models.LibreTranslate;
using Lexilearn.LibreTranslate.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Lexilearn.LibreTranslate.Services;

public class TranslationService : ITranslationService
{
    private readonly LibreTranslateSettings _settings;
    private readonly HttpClient _httpClient;

    public TranslationService(HttpClient httpClient, IOptions<LibreTranslateSettings> settings)
    {
        _settings = settings.Value;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri($"{_settings.Host.TrimEnd('/')}:{_settings.Port}");
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<TranslationResponse> TranslateText(TranslationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/translate", request);
        response.EnsureSuccessStatusCode();

        var translation = await response.Content.ReadFromJsonAsync<TranslationResponse>();
        if (translation is null)
            throw new InvalidOperationException("LibreTranslate returned an empty response.");

        return translation;
    }
}
