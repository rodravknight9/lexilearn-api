namespace Lexilearn.DataTransfer.Decks;

public class EditDeckRequest
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? TermLanguageCode { get; set; }
    public string? DefinitionLanguageCode { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
}