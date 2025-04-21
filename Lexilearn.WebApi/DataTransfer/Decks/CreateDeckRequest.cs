namespace Lexilearn.WebApi.DataTransfer.Decks;

public class CreateDeckRequest
{
    public string Title { get; set; }
    public string TermLanguageCode { get; set; }
    public string DefinitionLanguageCode { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
}