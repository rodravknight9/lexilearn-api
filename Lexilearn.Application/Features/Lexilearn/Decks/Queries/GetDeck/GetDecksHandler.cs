using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using Lexilearn.Application.Models.LexiLearn;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDeck
{
    public class GetDecksHandler : IRequestHandler<GetDeckQuery, Result<GetDeckResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetDecksHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<GetDeckResponse>> Handle(GetDeckQuery request, CancellationToken cancellationToken)
        {
            var deck = await _unitOfWork.DeckRepository.GetByIdAsync(request.Id);
            var result = _mapper.Map<GetDeckResponse>(deck);
            return Result<GetDeckResponse>.Success(result);
        }
    }
}
