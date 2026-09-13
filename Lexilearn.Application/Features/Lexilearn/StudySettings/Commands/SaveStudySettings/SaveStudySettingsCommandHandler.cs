using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.StudySettings.Commands.SaveStudySettings;

public class SaveStudySettingsCommandHandler : IRequestHandler<SaveStudySettingsCommand, SoftResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeckOwnershipService _ownership;

    public SaveStudySettingsCommandHandler(IUnitOfWork unitOfWork, IDeckOwnershipService ownership)
    {
        _unitOfWork = unitOfWork;
        _ownership = ownership;
    }

    public async Task<SoftResult> Handle(SaveStudySettingsCommand request, CancellationToken cancellationToken)
    {
        var deck = await _ownership.GetOwnedDeckAsync(request.DeckId, request.UserId, cancellationToken);
        if (deck is null)
            return SoftResult.Failure($"{Error.Forbidden.Code}: {Error.Forbidden.Message}");

        if (request.SessionSize is < 1 or > 100)
            return SoftResult.Failure("SessionSize must be between 1 and 100.");

        if (request.NewCardsPercentage is < 0 or > 100 || request.HardCardsPercentage is < 0 or > 100)
            return SoftResult.Failure("Percentages must be between 0 and 100.");

        if (request.NewCardsPercentage + request.HardCardsPercentage > 100)
            return SoftResult.Failure("NewCardsPercentage and HardCardsPercentage cannot add up to more than 100.");

        var settings = (await _unitOfWork.Repository<StudySessionSettings>()
                .GetMany(s => s.DeckId == request.DeckId))
            .FirstOrDefault();

        if (settings is null)
        {
            settings = new StudySessionSettings { DeckId = request.DeckId };
            await _unitOfWork.Repository<StudySessionSettings>().AddAsync(settings);
        }

        settings.SessionSize = request.SessionSize;
        settings.NewCardsPercentage = request.NewCardsPercentage;
        settings.HardCardsPercentage = request.HardCardsPercentage;

        await _unitOfWork.Complete();
        return SoftResult.Success();
    }
}
