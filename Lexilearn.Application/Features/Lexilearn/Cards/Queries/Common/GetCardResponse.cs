namespace Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;

public class GetCardResponse
{
    public int Id { get; set; }
    public string Front {  get; set; }
    public string Back {  get; set; }
    public string? Example { get; set; }
    public bool IsFavorite { get; set; }
    public int DeckId { get; set; }
    public int LastStatus { get; set; }
}