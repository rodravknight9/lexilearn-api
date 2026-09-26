using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Translation.Profiles.Common;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Profiles.Queries.GetTranslationProfiles;

public class GetTranslationProfilesHandler
    : IRequestHandler<GetTranslationProfilesQuery, Result<IReadOnlyList<TranslationProfileResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTranslationProfilesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IReadOnlyList<TranslationProfileResponse>>> Handle(
        GetTranslationProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var profiles = await _unitOfWork.Repository<TranslationProfile>()
            .GetMany(p => p.UserId == request.UserId && p.IsActive);

        var response = profiles
            .OrderBy(p => p.Name)
            .Select(TranslationProfileMapper.ToResponse)
            .ToList();

        return Result<IReadOnlyList<TranslationProfileResponse>>.Success(response);
    }
}
