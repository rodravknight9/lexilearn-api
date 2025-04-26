using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Shared;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetCardsByDeck;

public class GetDeckCardsByDeckHandler : IRequestHandler<GetCardsByDeckQuery, Result<IReadOnlyList<GetCardResponse>>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public GetDeckCardsByDeckHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<IReadOnlyList<GetCardResponse>>> Handle(GetCardsByDeckQuery request, CancellationToken cancellationToken)
    {
        var pagination = _mapper.Map<PaginationSettings>(request);
        var cards = await _unitOfWork.CardRepository.GetByDeckId(pagination, request.DeckId);
        var result = _mapper.Map<IReadOnlyList<GetCardResponse>>(cards);
        return Result<IReadOnlyList<GetCardResponse>>.Success(result);
    }
}