using System.Text.Json.Serialization;

namespace Lexilearn.DataTransfer.Translation
{
    public class TranslationOutput
    {
        [JsonPropertyName("translatedText")]
        public string TranslatedText { get; set; }
    }
}
