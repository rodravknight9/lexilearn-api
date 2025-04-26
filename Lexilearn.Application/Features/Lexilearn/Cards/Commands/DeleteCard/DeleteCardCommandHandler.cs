using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Commands.DeleteCard;

public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand, SoftResult>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteCardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<SoftResult> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _unitOfWork.Repository<Card>()
            .GetOne(card => card.Id == request.Id && card.CreatedBy == request.UserId);
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

        return SoftResult.Success();
    }
}