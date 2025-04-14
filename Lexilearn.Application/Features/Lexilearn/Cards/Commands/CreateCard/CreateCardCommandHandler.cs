using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Commands.CreateCard;

public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, CreateCardResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public CreateCardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<CreateCardResponse> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        var cardDomain = _mapper.Map<Card>(request);
        
        var newEntity = await _unitOfWork.Repository<Card>().AddAsync(cardDomain);
        await _unitOfWork.Complete();

        return new CreateCardResponse() { NewId = newEntity.Id };
    }
}