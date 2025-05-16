using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Models.LexiLearn;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.PracticeSession.Commands.SavePracticeSession;

public class SavePracticeSessionCommandHandler : IRequestHandler<SavePracticeSessionCommand, SoftResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public SavePracticeSessionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<SoftResult> Handle(SavePracticeSessionCommand request, CancellationToken cancellationToken)
    {
        var sessionDomain = _mapper.Map<Domain.PracticeSession>(request);
        await _unitOfWork.PracticeSessionRepository.AddAsync(sessionDomain);
        await _unitOfWork.Complete();
        return SoftResult.Success();
    }
}