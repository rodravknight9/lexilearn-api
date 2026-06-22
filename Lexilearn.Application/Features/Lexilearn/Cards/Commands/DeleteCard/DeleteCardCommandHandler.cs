using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Commands.DeleteCard;

public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand, SoftResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeckOwnershipService _ownership;

    public DeleteCardCommandHandler(IUnitOfWork unitOfWork, IDeckOwnershipService ownership)
    {
        _unitOfWork = unitOfWork;
        _ownership = ownership;
    }

    public async Task<SoftResult> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _ownership.GetOwnedCardAsync(request.Id, request.UserId, cancellationToken);
        if (card is null)
            return SoftResult.Failure($"{Error.NotFound.Code}: {Error.NotFound.Message}");

        if (card.IsActive)
        {
            card.IsActive = false;
            await _unitOfWork.Repository<Card>().UpdateAsync(card);
        }
        else
        {
            await _unitOfWork.Repository<Card>().DeleteAsync(card);
        }

        await _unitOfWork.Complete();
        return SoftResult.Success();
    }
}
