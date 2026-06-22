using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.PracticeSession.Queries.GetSessionHistory;

public class GetSessionHistoryHandler : IRequestHandler<GetSessionHistoryQuery, Result<List<GetSessionHistoryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSessionHistoryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<GetSessionHistoryResponse>>> Handle(GetSessionHistoryQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _unitOfWork.PracticeSessionRepository
            .GetMany(s => s.CreatedDate <= request.EndDate
                          && s.CreatedDate >= request.StartDate
                          && s.CreatedBy == request.UserId);

        var result = sessions.GroupBy(s => s.CreatedDate!.Value.Date)
            .Select(s => new GetSessionHistoryResponse
            {
                Date = s.Key,
                Records = s.Count()
            }).ToList();

        return Result<List<GetSessionHistoryResponse>>.Success(result);
    }
}
