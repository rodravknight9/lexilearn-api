using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDeck
{
    public class GetDeckQuery : IRequest<Result<GetDeckResponse>>
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
