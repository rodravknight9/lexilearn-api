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
        var existingDeck = await _unitOfWork.Repository<Deck>().GetByIdAsync(request.Id);

        if (existingDeck is null)
            throw new Exception("Deck not found");
        
        existingDeck = UpdateRequestedFields(request, existingDeck);
        await _unitOfWork.Repository<Deck>().UpdateAsync(existingDeck);
        await _unitOfWork.Complete();
    }

    private Deck UpdateRequestedFields(EditDeckCommand deckRequest, Deck deck)
    {
        deck.Description = deckRequest.Description ?? deck.Description;
        deck.Title = deckRequest.Title ?? deck.Title;
        deck.Color = deckRequest.Color ?? deck.Color;
        deck.DefinitionLanguageCode = deckRequest.DefinitionLanguageCode ?? deck.DefinitionLanguageCode;
        deck.TermLanguageCode = deckRequest.TermLanguageCode ?? deck.TermLanguageCode;
        deck.LastModifiedBy = deckRequest.LastModifiedBy;
        return deck;
    }
}