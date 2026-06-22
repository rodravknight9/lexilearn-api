using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Models.LexiLearn;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.PracticeSession.Commands.SavePracticeSession;

public class SavePracticeSessionCommandHandler : IRequestHandler<SavePracticeSessionCommand, SoftResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDeckOwnershipService _ownership;

    public SavePracticeSessionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDeckOwnershipService ownership)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _ownership = ownership;
    }

    public async Task<SoftResult> Handle(SavePracticeSessionCommand request, CancellationToken cancellationToken)
    {
        var deck = await _ownership.GetOwnedDeckAsync(request.DeckId, request.CreatedBy, cancellationToken);
        if (deck is null)
            return SoftResult.Failure($"{Error.Forbidden.Code}: {Error.Forbidden.Message}");

        foreach (var card in request.Cards)
        {
            var ownedCard = await _ownership.GetOwnedCardAsync(card.CardId, request.CreatedBy, cancellationToken);
            if (ownedCard is null || ownedCard.DeckId != request.DeckId)
                return SoftResult.Failure($"{Error.Forbidden.Code}: {Error.Forbidden.Message}");
        }

        var sessionDomain = _mapper.Map<Domain.PracticeSession>(request);
        await _unitOfWork.PracticeSessionRepository.AddAsync(sessionDomain);
        await _unitOfWork.Complete();
        return SoftResult.Success();
    }
}
