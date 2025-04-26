using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Shared;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDecks
{
    public class GetDecksQuery : PaginationSettings, IRequest<Result<List<GetDeckResponse>>>
    {
        public int UserId { get; set; }
    }
}
