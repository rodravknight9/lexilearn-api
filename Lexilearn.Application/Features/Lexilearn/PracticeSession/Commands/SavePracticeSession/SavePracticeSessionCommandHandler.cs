using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.PracticeSession.Commands.SavePracticeSession;

public class SavePracticeSessionCommandHandler : IRequestHandler<SavePracticeSessionCommand, SoftResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeckOwnershipService _ownership;
    private readonly ISpacedRepetitionScheduler _scheduler;

    public SavePracticeSessionCommandHandler(IUnitOfWork unitOfWork, IDeckOwnershipService ownership, ISpacedRepetitionScheduler scheduler)
    {
        _unitOfWork = unitOfWork;
        _ownership = ownership;
        _scheduler = scheduler;
    }

    public async Task<SoftResult> Handle(SavePracticeSessionCommand request, CancellationToken cancellationToken)
    {
        var deck = await _ownership.GetOwnedDeckAsync(request.DeckId, request.CreatedBy, cancellationToken);
        if (deck is null)
            return SoftResult.Failure($"{Error.Forbidden.Code}: {Error.Forbidden.Message}");

        var schedulingStates = new Dictionary<int, CardSchedulingState>();
        foreach (var cardId in request.Cards.Select(c => c.CardId).Distinct())
        {
            var ownedCard = await _ownership.GetOwnedCardAsync(cardId, request.CreatedBy, cancellationToken);
            if (ownedCard is null || ownedCard.DeckId != request.DeckId)
                return SoftResult.Failure($"{Error.Forbidden.Code}: {Error.Forbidden.Message}");

            schedulingStates[cardId] = await _unitOfWork.Repository<CardSchedulingState>()
                .GetOne(s => s.CardId == cardId);
        }

        var sessionDomain = new Domain.PracticeSession
        {
            DeckId = request.DeckId,
            Cards = new List<CardReview>()
        };

        var now = DateTime.UtcNow;
        foreach (var attempt in request.Cards)
        {
            var state = schedulingStates[attempt.CardId];
            var previousStatus = state.Status;
            var previousReviewAt = state.NextReviewAt;

            _scheduler.ApplyReview(state, attempt.Rating, now);

            sessionDomain.Cards.Add(new CardReview
            {
                CardId = attempt.CardId,
                Rating = attempt.Rating,
                ReviewedAt = now,
                PreviousStatus = previousStatus,
                NextStatus = state.Status,
                PreviousReviewAt = previousReviewAt,
                NextReviewAt = state.NextReviewAt
            });

            await _unitOfWork.Repository<CardSchedulingState>().UpdateAsync(state);
        }

        await _unitOfWork.PracticeSessionRepository.AddAsync(sessionDomain);
        await _unitOfWork.Complete();
        return SoftResult.Success();
    }
}
