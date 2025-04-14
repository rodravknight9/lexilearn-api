using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Commands.CreateCard;

public class CreateCardCommand : IRequest<CreateCardResponse>
{
    public required string Front {  get; set; }
    public required string Back {  get; set; }
    public bool IsFavorite { get; set; }
    public int DeckId { get; set; }
}