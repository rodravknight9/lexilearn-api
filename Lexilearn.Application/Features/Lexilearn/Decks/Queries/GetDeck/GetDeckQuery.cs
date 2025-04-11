using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDeck
{
    public class GetDeckQuery : IRequest<GetDeckResponse>
    {
        public int Id { get; set; }
        public GetDeckQuery(int id)
        {
            Id = id;
        }
    }
}
