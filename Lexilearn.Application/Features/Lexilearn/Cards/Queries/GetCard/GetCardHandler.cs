using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetCard;

public class GetCardHandler : IRequestHandler<GetCardQuery, Result<GetCardResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GetCardHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<GetCardResponse>> Handle(GetCardQuery request, CancellationToken cancellationToken)
    {
        var card = await _unitOfWork.Repository<Card>().GetOne(card => card.Id == request.Id);
        var result = _mapper.Map<GetCardResponse>(card);
        return Result<GetCardResponse>.Success(result);
    }
}