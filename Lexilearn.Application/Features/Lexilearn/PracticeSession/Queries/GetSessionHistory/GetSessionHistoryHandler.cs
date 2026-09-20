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
        // EndDate arrives as a bare date (midnight). Comparing with <= would exclude every
        // session created later that same day, so the range must extend through the end of
        // EndDate's calendar day instead of stopping at its start.
        var exclusiveEndDate = request.EndDate.Date.AddDays(1);

        var sessions = await _unitOfWork.PracticeSessionRepository
            .GetMany(s => s.CreatedDate < exclusiveEndDate
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
