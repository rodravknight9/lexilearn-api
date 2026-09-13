using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;
using Lexilearn.Application.Features.Lexilearn.StudySettings.Common;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using Lexilearn.Domain.Enums;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetDueCards;

public class GetDueCardsHandler : IRequestHandler<GetDueCardsQuery, Result<IReadOnlyList<GetCardResponse>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeckOwnershipService _ownership;

    public GetDueCardsHandler(IMapper mapper, IUnitOfWork unitOfWork, IDeckOwnershipService ownership)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _ownership = ownership;
    }

    public async Task<Result<IReadOnlyList<GetCardResponse>>> Handle(GetDueCardsQuery request, CancellationToken cancellationToken)
    {
        var deck = await _ownership.GetOwnedDeckAsync(request.DeckId, request.UserId, cancellationToken);
        if (deck is null)
            return Result<IReadOnlyList<GetCardResponse>>.Failure(Error.NotFound);

        var cards = await _unitOfWork.Repository<Card>().GetMany(c => c.DeckId == request.DeckId && c.IsActive);
        var cardIds = cards.Select(c => c.Id).ToList();
        var states = cardIds.Count == 0
            ? Array.Empty<CardSchedulingState>()
            : await _unitOfWork.Repository<CardSchedulingState>().GetMany(s => cardIds.Contains(s.CardId));
        var statesByCardId = states.ToDictionary(s => s.CardId);

        var savedSettings = (await _unitOfWork.Repository<StudySessionSettings>()
                .GetMany(s => s.DeckId == request.DeckId))
            .FirstOrDefault();

        var limit = request.Limit
            ?? savedSettings?.SessionSize
            ?? StudySettingsDefaults.SessionSize;
        var newCardsPercentage = request.NewCardsPercentage
            ?? savedSettings?.NewCardsPercentage
            ?? StudySettingsDefaults.NewCardsPercentage;
        var hardCardsPercentage = request.HardCardsPercentage
            ?? savedSettings?.HardCardsPercentage
            ?? StudySettingsDefaults.HardCardsPercentage;

        var now = DateTime.UtcNow;

        var newCards = cards
            .Where(c => statesByCardId[c.Id].Status == SchedulingStatus.New)
            .OrderBy(c => c.CreatedDate)
            .ToList();

        var dueCards = cards
            .Where(c => statesByCardId[c.Id].Status is SchedulingStatus.Learning or SchedulingStatus.Review or SchedulingStatus.Learnt
                        && statesByCardId[c.Id].NextReviewAt <= now)
            .ToList();

        var hardDueCards = dueCards
            .Where(c => statesByCardId[c.Id].Status == SchedulingStatus.Learning)
            .OrderBy(c => statesByCardId[c.Id].NextReviewAt)
            .ToList();

        var otherDueCards = dueCards
            .Except(hardDueCards)
            .OrderBy(c => statesByCardId[c.Id].NextReviewAt)
            .ToList();

        var hardSlots = (int)Math.Round(limit * hardCardsPercentage / 100.0);
        var newSlots = (int)Math.Round(limit * newCardsPercentage / 100.0);

        var selected = new List<Card>();
        selected.AddRange(hardDueCards.Take(hardSlots));
        selected.AddRange(newCards.Take(newSlots));

        var remainingSlots = Math.Max(0, limit - selected.Count);
        selected.AddRange(otherDueCards.Take(remainingSlots));

        if (selected.Count < limit)
        {
            var selectedIds = selected.Select(c => c.Id).ToHashSet();
            var leftovers = hardDueCards.Concat(newCards).Concat(otherDueCards)
                .Where(c => !selectedIds.Contains(c.Id));

            foreach (var card in leftovers)
            {
                if (selected.Count >= limit)
                    break;

                selected.Add(card);
                selectedIds.Add(card.Id);
            }
        }

        var result = _mapper.Map<IReadOnlyList<GetCardResponse>>(selected);
        return Result<IReadOnlyList<GetCardResponse>>.Success(result);
    }
}
