using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Shared;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDecks
{
    public class GetDeckHandler : IRequestHandler<GetDecksQuery, Result<List<GetDeckResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetDeckHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<List<GetDeckResponse>>> Handle(GetDecksQuery request, CancellationToken cancellationToken)
        {
            var pagination = _mapper.Map<PaginationSettings>(request);
            var decks = 
                await _unitOfWork.DeckRepository.GetAsync(pagination, 
                    deck => deck.CreatedBy.Equals(request.UserId) && deck.IsActive);
            var result = _mapper.Map<List<GetDeckResponse>>(decks);
            return Result<List<GetDeckResponse>>.Success(result);
        }
    }
}
