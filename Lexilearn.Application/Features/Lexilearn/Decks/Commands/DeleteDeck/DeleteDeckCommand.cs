using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.DeleteDeck;

public class DeleteDeckCommand : IRequest
{
    public int Id { get; set; }

    public DeleteDeckCommand(int id)
    {
        Id = id;
    }
}