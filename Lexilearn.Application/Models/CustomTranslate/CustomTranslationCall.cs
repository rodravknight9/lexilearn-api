namespace Lexilearn.Application.Models.CustomTranslate;

public class CustomTranslationCall
{
    public string Url { get; set; } = null!;
    public string HttpMethod { get; set; } = "POST";
    public IReadOnlyList<RequestEntry> Headers { get; set; } = [];
    public IReadOnlyList<RequestEntry> Body { get; set; } = [];
    public string ResponsePath { get; set; } = null!;
    public string Text { get; set; } = null!;
    public string Source { get; set; } = null!;
    public string Target { get; set; } = null!;
}
