using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDeck
{
    public class GetDeckQuery : IRequest<GetDeckResponse>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public GetDeckQuery(int id, int userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
