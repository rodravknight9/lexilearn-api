namespace Lexilearn.DataTransfer.Cards;

public class CreateCardRequest
{
    public required string Front {  get; set; }
    public required string Back {  get; set; }
    public bool IsFavorite { get; set; }
    public int DeckId { get; set; }
}