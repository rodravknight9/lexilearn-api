using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.DeleteDeck;

public class DeleteDeckCommandHandler : IRequestHandler<DeleteDeckCommand, SoftResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDeckOwnershipService _ownership;

    public DeleteDeckCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDeckOwnershipService ownership)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _ownership = ownership;
    }

    public async Task<SoftResult> Handle(DeleteDeckCommand request, CancellationToken cancellationToken)
    {
        var deck = await _ownership.GetOwnedDeckAsync(request.Id, request.UserId, cancellationToken);
        if (deck is null)
            return SoftResult.Failure($"{Error.NotFound.Code}: {Error.NotFound.Message}");

        if (deck.IsActive)
        {
            deck.IsActive = false;
            await _unitOfWork.Repository<Deck>().UpdateAsync(deck);
        }
        else
        {
            await _unitOfWork.Repository<Deck>().DeleteAsync(deck);
        }

        await _unitOfWork.Complete();
        return SoftResult.Success();
    }
}
