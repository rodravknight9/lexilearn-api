using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using Lexilearn.Shared;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDecks
{
    public class GetDecksQuery : PaginationSettings, IRequest<List<GetDeckResponse>>
    {
    }
}
