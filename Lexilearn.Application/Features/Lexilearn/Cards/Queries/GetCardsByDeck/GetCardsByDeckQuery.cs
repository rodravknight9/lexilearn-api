using Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;
using Lexilearn.Shared;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetCardsByDeck;

public class GetCardsByDeckQuery : PaginationSettings, IRequest<IReadOnlyList<GetCardResponse>>
{
    public int DeckId { get; set; }

    public GetCardsByDeckQuery()
    {
    }

    public GetCardsByDeckQuery(int deckId)
    {
        DeckId = deckId;
    }
}