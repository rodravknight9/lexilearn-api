using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Domain;
using MapsterMapper;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Commands.EditCard;

public class EditCardCommandHandler : IRequestHandler<EditCardCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public EditCardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(EditCardCommand request, CancellationToken cancellationToken)
    {
        var domain = _mapper.Map<Card>(request);
        await _unitOfWork.Repository<Card>().UpdateAsync(domain);
        await _unitOfWork.Complete();
    }
}