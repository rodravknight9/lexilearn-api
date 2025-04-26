namespace Lexilearn.WebApi.DataTransfer.Cards;

public class EditCardRequest
{
    public int Id { get; set; }
    public string? Front {  get; set; }
    public string? Back {  get; set; }
    public bool? IsFavorite { get; set; }
    public int? DeckId { get; set; }    
}