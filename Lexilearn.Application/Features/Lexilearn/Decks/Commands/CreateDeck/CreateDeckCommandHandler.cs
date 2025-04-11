using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.CreateDeck
{
    public class CreateDeckCommandHandler : IRequestHandler<CreateDeckCommand, CreateDeckResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateDeckCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateDeckResponse> Handle(CreateDeckCommand request, CancellationToken cancellationToken)
        {
            var deckDomain = _mapper.Map<Deck>(request);

            var newEntity = await _unitOfWork.Repository<Deck>().AddAsync(deckDomain);
            await _unitOfWork.Complete();

            return new CreateDeckResponse { NewId = newEntity.Id };
        }
    }
}
