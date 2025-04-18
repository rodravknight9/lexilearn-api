using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Domain;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Commands.DeleteCard;

public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteCardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _unitOfWork.Repository<Card>().GetByIdAsync(request.Id);
        if (card.IsActive)
        {
            // soft delete
            card.IsActive = false;
            await _unitOfWork.Repository<Card>().UpdateAsync(card);
        }
        else
        {
            await _unitOfWork.Repository<Card>().DeleteAsync(card);
        }
    }
}