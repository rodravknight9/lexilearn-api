using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.CreateDeck
{
    public class CreateDeckCommandHandler : IRequestHandler<CreateDeckCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateDeckCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CreateDeckCommand request, CancellationToken cancellationToken)
        {
            var deckDomain = _mapper.Map<Deck>(request);

            await _unitOfWork.Repository<Deck>().AddAsync(deckDomain);
            var result = await _unitOfWork.Complete();

            return result;
        }
    }
}
