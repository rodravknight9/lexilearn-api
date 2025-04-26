using Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;
using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetCard;

public class GetCardQuery : IRequest<Result<GetCardResponse>>
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public GetCardQuery(int id, int userId)
    {
        Id = id;
        UserId = userId;
    }
}