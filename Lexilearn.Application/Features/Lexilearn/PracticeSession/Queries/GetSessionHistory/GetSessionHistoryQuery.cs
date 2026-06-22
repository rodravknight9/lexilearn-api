using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.PracticeSession.Queries.GetSessionHistory;

public class GetSessionHistoryQuery : IRequest<Result<List<GetSessionHistoryResponse>>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UserId { get; set; }
}