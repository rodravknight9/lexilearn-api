using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Commands.DeleteCard;

public class DeleteCardCommand : IRequest<SoftResult>
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public DeleteCardCommand(int id, int userId)
    {
        Id = id;
        UserId = userId;
    }
}