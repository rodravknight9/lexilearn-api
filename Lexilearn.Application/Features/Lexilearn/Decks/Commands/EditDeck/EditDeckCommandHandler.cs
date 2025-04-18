using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Lexilearn.Decks.Commands.CreateDeck;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.EditDeck;

public class EditDeckCommandHandler : IRequestHandler<EditDeckCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public EditDeckCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task Handle(EditDeckCommand request, CancellationToken cancellationToken)
    {
        var deckDomain = _mapper.Map<Deck>(request);
        await _unitOfWork.Repository<Deck>().UpdateAsync(deckDomain);
        await _unitOfWork.Complete();
    }
}