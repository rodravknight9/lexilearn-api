using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Commands.DeleteCard;

public class DeleteCardCommand : IRequest
{
    public int Id { get; set; }

    public DeleteCardCommand(int id)
    {
        Id = id;
    }
}