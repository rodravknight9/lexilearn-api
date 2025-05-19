using Lexilearn.Web.Models;
using Lexilearn.Web.Services.Interfaces;
using System.Text.Json;
using System.Text;
using Lexilearn.DataTransfer.Translation;

namespace Lexilearn.Web.Services.Implementation
{
    public class TranslationService : ITranslationService
    {
        private readonly HttpClient _http;
        public TranslationService()
        {
            _http = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5288"),
            };
        }
        public async Task<TranslationOutput> Translate(TranslationModel model)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _http.PostAsync("/api/Translation", jsonContent);
            var response2 = await response.Content.ReadAsStringAsync();
            var test = JsonSerializer.Deserialize<TranslationOutput>(response2)!;
            return JsonSerializer.Deserialize<TranslationOutput>(response2)!;
        }
    }
}
