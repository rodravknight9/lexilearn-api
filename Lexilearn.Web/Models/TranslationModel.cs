namespace Lexilearn.Web.Models;

public class TranslationModel
{
    public string Text { get; set; }
    public string LanguageSourceCode { get; set; } = "en";
    public string LanguageTargetCode { get; set; } = "es";
}
