using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using Lexilearn.Shared;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetCardsByDeck;

public class GetDeckCardsByDeckHandler : IRequestHandler<GetCardsByDeckQuery, Result<IReadOnlyList<GetCardResponse>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeckOwnershipService _ownership;

    public GetDeckCardsByDeckHandler(IMapper mapper, IUnitOfWork unitOfWork, IDeckOwnershipService ownership)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _ownership = ownership;
    }

    public async Task<Result<IReadOnlyList<GetCardResponse>>> Handle(GetCardsByDeckQuery request, CancellationToken cancellationToken)
    {
        var deck = await _ownership.GetOwnedDeckAsync(request.DeckId, request.UserId, cancellationToken);
        if (deck is null)
            return Result<IReadOnlyList<GetCardResponse>>.Failure(Error.NotFound);

        var pagination = _mapper.Map<PaginationSettings>(request);
        var cards = await _unitOfWork.CardRepository.GetByDeckId(pagination, request.DeckId);

        var cardIds = cards.Select(c => c.Id).ToList();
        var sessionCards = cardIds.Count == 0
            ? Array.Empty<PracticeSessionCards>()
            : await _unitOfWork.PracticeSessionCardsRepository.GetMany(s => cardIds.Contains(s.CardId));

        var sessionIds = sessionCards.Select(s => s.SessionId).Distinct().ToList();
        var sessions = sessionIds.Count == 0
            ? Array.Empty<Domain.PracticeSession>()
            : await _unitOfWork.PracticeSessionRepository.GetMany(s => sessionIds.Contains(s.Id));

        var sessionDates = sessions.ToDictionary(s => s.Id, s => s.CreatedDate ?? DateTime.MinValue);

        var result = _mapper.Map<IReadOnlyList<GetCardResponse>>(cards);
        foreach (var cardResponse in result)
        {
            cardResponse.LastStatus = sessionCards
                .Where(s => s.CardId == cardResponse.Id)
                .OrderByDescending(s => sessionDates.GetValueOrDefault(s.SessionId))
                .Select(s => s.Status)
                .FirstOrDefault();
        }

        return Result<IReadOnlyList<GetCardResponse>>.Success(result);
    }
}
