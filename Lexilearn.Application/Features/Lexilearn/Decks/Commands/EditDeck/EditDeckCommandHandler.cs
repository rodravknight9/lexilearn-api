using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.EditDeck;

public class EditDeckCommandHandler : IRequestHandler<EditDeckCommand, SoftResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public EditDeckCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<SoftResult> Handle(EditDeckCommand request, CancellationToken cancellationToken)
    {
        var existingDeck = await _unitOfWork.Repository<Deck>().GetByIdAsync(request.Id);

        if (existingDeck is null)
            return SoftResult.Failure($"Deck with id {request.Id} does not exist");
        
        existingDeck = UpdateRequestedFields(request, existingDeck);
        await _unitOfWork.Repository<Deck>().UpdateAsync(existingDeck);
        await _unitOfWork.Complete();
        
        return SoftResult.Success();
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