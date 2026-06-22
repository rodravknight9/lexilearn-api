using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;
using Lexilearn.Application.Models.LexiLearn;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetCard;

public class GetCardHandler : IRequestHandler<GetCardQuery, Result<GetCardResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDeckOwnershipService _ownership;

    public GetCardHandler(IUnitOfWork unitOfWork, IMapper mapper, IDeckOwnershipService ownership)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _ownership = ownership;
    }

    public async Task<Result<GetCardResponse>> Handle(GetCardQuery request, CancellationToken cancellationToken)
    {
        var card = await _ownership.GetOwnedCardAsync(request.Id, request.UserId, cancellationToken);
        if (card is null)
            return Result<GetCardResponse>.Failure(Error.NotFound);

        var result = _mapper.Map<GetCardResponse>(card);
        return Result<GetCardResponse>.Success(result);
    }
}
