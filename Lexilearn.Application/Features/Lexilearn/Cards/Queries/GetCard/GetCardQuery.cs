using Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetCard;

public class GetCardQuery : IRequest<GetCardResponse>
{
    public int Id { get; set; }

    public GetCardQuery(int id)
    {
        Id = id;
    }
}