using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using Lexilearn.Shared;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDecks
{
    public class GetDeckHandler : IRequestHandler<GetDecksQuery, List<GetDeckResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetDeckHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<GetDeckResponse>> Handle(GetDecksQuery request, CancellationToken cancellationToken)
        {
            var pagination = _mapper.Map<PaginationSettings>(request);
            var decks = 
                await _unitOfWork.DeckRepository.GetAsync(pagination, deck => deck.CreatedBy.Equals(request.UserId));
            return _mapper.Map<List<GetDeckResponse>>(decks);
        }
    }
}
