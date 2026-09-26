using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Profiles.Commands.DeleteTranslationProfile;

public class DeleteTranslationProfileCommandHandler : IRequestHandler<DeleteTranslationProfileCommand, SoftResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTranslationProfileCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SoftResult> Handle(DeleteTranslationProfileCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<TranslationProfile>();
        var profile = (await repository.GetMany(p =>
                p.Id == request.Id && p.UserId == request.UserId && p.IsActive))
            .FirstOrDefault();

        if (profile is null)
            return SoftResult.Failure(Error.NotFound);

        profile.IsActive = false;
        profile.IsDefault = false;
        profile.LastModifiedBy = request.UserId;
        await repository.UpdateAsync(profile);
        await _unitOfWork.Complete();
        return SoftResult.Success();
    }
}
