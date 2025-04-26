using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.CreateDeck
{
    public class CreateDeckCommandHandler : IRequestHandler<CreateDeckCommand, Result<CreateDeckResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateDeckCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<CreateDeckResponse>> Handle(CreateDeckCommand request, CancellationToken cancellationToken)
        {
            var deckDomain = _mapper.Map<Deck>(request);

            var newEntity = await _unitOfWork.Repository<Deck>().AddAsync(deckDomain);
            await _unitOfWork.Complete();
            
            var response = new CreateDeckResponse { NewId = newEntity.Id };
            return Result<CreateDeckResponse>.Success(response);
        }
    }
}
