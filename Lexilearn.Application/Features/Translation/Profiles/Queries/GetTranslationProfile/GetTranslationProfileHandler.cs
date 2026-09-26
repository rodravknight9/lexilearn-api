using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Translation.Profiles.Common;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Profiles.Queries.GetTranslationProfile;

public class GetTranslationProfileHandler : IRequestHandler<GetTranslationProfileQuery, Result<TranslationProfileResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTranslationProfileHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TranslationProfileResponse>> Handle(
        GetTranslationProfileQuery request,
        CancellationToken cancellationToken)
    {
        var profile = (await _unitOfWork.Repository<TranslationProfile>()
                .GetMany(p => p.Id == request.Id && p.UserId == request.UserId && p.IsActive))
            .FirstOrDefault();

        if (profile is null)
            return Result<TranslationProfileResponse>.Failure(Error.NotFound);

        return Result<TranslationProfileResponse>.Success(TranslationProfileMapper.ToResponse(profile));
    }
}
