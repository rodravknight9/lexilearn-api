using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDeck
{
    public class GetDecksHandler : IRequestHandler<GetDeckQuery, GetDeckResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetDecksHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetDeckResponse> Handle(GetDeckQuery request, CancellationToken cancellationToken)
        {
            var deck = await _unitOfWork.DeckRepository.GetByIdAsync(request.Id);
            return _mapper.Map<GetDeckResponse>(deck);
        }
    }
}
