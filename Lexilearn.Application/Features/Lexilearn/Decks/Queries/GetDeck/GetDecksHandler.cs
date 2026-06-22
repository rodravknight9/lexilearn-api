using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using Lexilearn.Application.Models.LexiLearn;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDeck;

public class GetDecksHandler : IRequestHandler<GetDeckQuery, Result<GetDeckResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDeckOwnershipService _ownership;

    public GetDecksHandler(IUnitOfWork unitOfWork, IMapper mapper, IDeckOwnershipService ownership)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _ownership = ownership;
    }

    public async Task<Result<GetDeckResponse>> Handle(GetDeckQuery request, CancellationToken cancellationToken)
    {
        var deck = await _ownership.GetOwnedDeckAsync(request.Id, request.UserId, cancellationToken);
        if (deck is null)
            return Result<GetDeckResponse>.Failure(Error.NotFound);

        var result = _mapper.Map<GetDeckResponse>(deck);
        return Result<GetDeckResponse>.Success(result);
    }
}
