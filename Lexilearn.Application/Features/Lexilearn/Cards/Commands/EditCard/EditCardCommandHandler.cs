using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Commands.EditCard;

public class EditCardCommandHandler : IRequestHandler<EditCardCommand, SoftResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public EditCardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<SoftResult> Handle(EditCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _unitOfWork.Repository<Card>().GetByIdAsync(request.Id);

        if (card is null)
            return SoftResult.Failure("Not Found");
        
        card = UpdateRequestedFields(request, card);
        await _unitOfWork.Repository<Card>().UpdateAsync(card);
        await _unitOfWork.Complete();
        
        return SoftResult.Success();
    }
    
    private Card UpdateRequestedFields(EditCardCommand cardRequest, Card card)
    {
        card.Front = cardRequest.Front ?? card.Front;
        card.Back = cardRequest.Back ?? card.Back;
        card.IsFavorite = cardRequest.IsFavorite ?? card.IsFavorite;
        card.DeckId = cardRequest.DeckId ?? card.DeckId;
        card.LastModifiedBy = cardRequest.LastModifiedBy;
        return card;
    }
}