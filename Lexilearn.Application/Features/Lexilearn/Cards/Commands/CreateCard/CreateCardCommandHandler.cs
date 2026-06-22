using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Contracts.Services;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Commands.CreateCard;

public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, Result<CreateCardResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDeckOwnershipService _ownership;

    public CreateCardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDeckOwnershipService ownership)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _ownership = ownership;
    }

    public async Task<Result<CreateCardResponse>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        var deck = await _ownership.GetOwnedDeckAsync(request.DeckId, request.CreatedBy, cancellationToken);
        if (deck is null)
            return Result<CreateCardResponse>.Failure(Error.Forbidden);

        var cardDomain = _mapper.Map<Card>(request);

        var newEntity = await _unitOfWork.Repository<Card>().AddAsync(cardDomain);
        await _unitOfWork.Complete();

        var result = new CreateCardResponse { NewId = newEntity.Id };
        return Result<CreateCardResponse>.Success(result);
    }
}
