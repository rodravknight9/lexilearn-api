namespace Lexilearn.Application.Models.LibreTranslate
{
    public class TranslationRequest
    {
        /// <summary>
        /// The text to be translated
        /// </summary>
        public string q { get; set; } = null!;
        /// <summary>
        /// The Source Language Code
        /// </summary>
        public string source { get; set; } = null!;
        /// <summary>
        /// The Target Language Code
        /// </summary>
        public string target { get; set; } = null!;
    }
}
