using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetCard;

public class GetCardHandler : IRequestHandler<GetCardQuery, GetCardResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GetCardHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<GetCardResponse> Handle(GetCardQuery request, CancellationToken cancellationToken)
    {
        var card = await _unitOfWork.Repository<Card>().GetByIdAsync(request.Id);
        return _mapper.Map<GetCardResponse>(card);
    }
}