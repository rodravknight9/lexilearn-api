using Lexilearn.Application.Contracts.Infastructure;
using Lexilearn.Application.Models.LibreTranslate;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using Lexilearn.LibreTranslate.Models;

namespace Lexilearn.LibreTranslate.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly LibreTranslateSettings _settings;
        private readonly HttpClient _httpClient;
        public TranslationService(IOptions<LibreTranslateSettings> settings)
        {
            _settings = settings.Value;
            _httpClient = new HttpClient();
        }

        public async Task<TranslationResponse> TranslateText(TranslationRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = GetLibreTranslateHost();

            var response = await _httpClient.PostAsync(url, data);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var translation = JsonSerializer.Deserialize<TranslationResponse>(jsonResponse);
            
            return translation;
        }

        private string GetLibreTranslateHost() 
        {
            return _settings.Host + ":" + _settings.Port + "/translate";
        }

    }
}
