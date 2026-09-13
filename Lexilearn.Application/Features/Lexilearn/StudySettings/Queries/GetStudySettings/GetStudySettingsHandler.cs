using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Features.Lexilearn.StudySettings.Common;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.StudySettings.Queries.GetStudySettings;

public class GetStudySettingsHandler : IRequestHandler<GetStudySettingsQuery, Result<StudySettingsResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeckOwnershipService _ownership;

    public GetStudySettingsHandler(IUnitOfWork unitOfWork, IDeckOwnershipService ownership)
    {
        _unitOfWork = unitOfWork;
        _ownership = ownership;
    }

    public async Task<Result<StudySettingsResponse>> Handle(GetStudySettingsQuery request, CancellationToken cancellationToken)
    {
        var deck = await _ownership.GetOwnedDeckAsync(request.DeckId, request.UserId, cancellationToken);
        if (deck is null)
            return Result<StudySettingsResponse>.Failure(Error.NotFound);

        var settings = (await _unitOfWork.Repository<StudySessionSettings>()
                .GetMany(s => s.DeckId == request.DeckId))
            .FirstOrDefault();

        var response = new StudySettingsResponse
        {
            SessionSize = settings?.SessionSize ?? StudySettingsDefaults.SessionSize,
            NewCardsPercentage = settings?.NewCardsPercentage ?? StudySettingsDefaults.NewCardsPercentage,
            HardCardsPercentage = settings?.HardCardsPercentage ?? StudySettingsDefaults.HardCardsPercentage
        };

        return Result<StudySettingsResponse>.Success(response);
    }
}
