using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Commands.EditCard;

public class EditCardCommandHandler : IRequestHandler<EditCardCommand, SoftResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDeckOwnershipService _ownership;

    public EditCardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDeckOwnershipService ownership)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _ownership = ownership;
    }

    public async Task<SoftResult> Handle(EditCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _ownership.GetOwnedCardAsync(request.Id, request.UserId, cancellationToken);
        if (card is null)
            return SoftResult.Failure($"{Error.NotFound.Code}: {Error.NotFound.Message}");

        if (request.DeckId.HasValue && request.DeckId.Value != card.DeckId)
        {
            var targetDeck = await _ownership.GetOwnedDeckAsync(request.DeckId.Value, request.UserId, cancellationToken);
            if (targetDeck is null)
                return SoftResult.Failure($"{Error.Forbidden.Code}: {Error.Forbidden.Message}");
        }

        card = UpdateRequestedFields(request, card);
        await _unitOfWork.Repository<Card>().UpdateAsync(card);
        await _unitOfWork.Complete();

        return SoftResult.Success();
    }

    private static Card UpdateRequestedFields(EditCardCommand cardRequest, Card card)
    {
        card.Front = cardRequest.Front ?? card.Front;
        card.Back = cardRequest.Back ?? card.Back;
        card.IsFavorite = cardRequest.IsFavorite ?? card.IsFavorite;
        card.DeckId = cardRequest.DeckId ?? card.DeckId;
        card.LastModifiedBy = cardRequest.LastModifiedBy;
        return card;
    }
}
