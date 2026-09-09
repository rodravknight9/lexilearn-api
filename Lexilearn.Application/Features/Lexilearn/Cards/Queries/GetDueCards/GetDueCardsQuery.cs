using Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;
using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetDueCards;

public class GetDueCardsQuery : IRequest<Result<IReadOnlyList<GetCardResponse>>>
{
    public int DeckId { get; set; }
    public int UserId { get; set; }

    public int Limit { get; set; } = 20;
    public int NewCardsPercentage { get; set; } = 20;
    public int HardCardsPercentage { get; set; } = 30;
}
