using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.DeleteDeck;

public class DeleteDeckCommandHandler : IRequestHandler<DeleteDeckCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public DeleteDeckCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(DeleteDeckCommand request, CancellationToken cancellationToken)
    {
        var deck = await _unitOfWork.Repository<Deck>().GetByIdAsync(request.Id);
        if (deck.IsActive)
        {
            // soft delete
            deck.IsActive = false;
            await _unitOfWork.Repository<Deck>().UpdateAsync(deck);
        }
        else
        {
            await _unitOfWork.Repository<Deck>().DeleteAsync(deck);
        }
    }
}