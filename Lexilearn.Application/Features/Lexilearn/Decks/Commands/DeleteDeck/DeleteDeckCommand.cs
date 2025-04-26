using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.DeleteDeck;

public class DeleteDeckCommand : IRequest<SoftResult>
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public DeleteDeckCommand(int id, int userId)
    {
        Id = id;
        UserId = userId;
    }
}