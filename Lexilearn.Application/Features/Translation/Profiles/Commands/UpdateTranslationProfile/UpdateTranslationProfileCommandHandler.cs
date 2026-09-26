using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Translation.Profiles.Common;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using Lexilearn.Domain.Enums;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Profiles.Commands.UpdateTranslationProfile;

public class UpdateTranslationProfileCommandHandler
    : IRequestHandler<UpdateTranslationProfileCommand, Result<TranslationProfileResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTranslationProfileCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TranslationProfileResponse>> Handle(
        UpdateTranslationProfileCommand request,
        CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<TranslationProfile>();
        var profile = (await repository.GetMany(p =>
                p.Id == request.Id && p.UserId == request.UserId && p.IsActive))
            .FirstOrDefault();

        if (profile is null)
            return Result<TranslationProfileResponse>.Failure(Error.NotFound);

        if (profile.Kind == TranslationProfileKind.LibreTranslate)
            return await UpdateLibreTranslate(request, repository, profile);

        var validated = ValidatedTranslationProfile.Validate(
            request.Name,
            request.Url,
            request.HttpMethod,
            request.Headers,
            request.Body,
            request.ResponsePath);

        if (!validated.IsValid)
            return Result<TranslationProfileResponse>.Failure(validated.Error!);

        if (request.IsDefault)
            await TranslationProfileDefaults.ClearAsync(repository, request.UserId, profile.Id);

        TranslationProfileMapper.Apply(profile, validated, request.IsDefault);
        profile.LastModifiedBy = request.UserId;
        await repository.UpdateAsync(profile);
        await _unitOfWork.Complete();

        return Result<TranslationProfileResponse>.Success(TranslationProfileMapper.ToResponse(profile));
    }

    private async Task<Result<TranslationProfileResponse>> UpdateLibreTranslate(
        UpdateTranslationProfileCommand request,
        IAsyncRepository<TranslationProfile> repository,
        TranslationProfile profile)
    {
        var urlError = LibreTranslateProfile.ValidateUrl(request.Url);
        if (urlError is not null)
            return Result<TranslationProfileResponse>.Failure(urlError);

        if (request.IsDefault)
            await TranslationProfileDefaults.ClearAsync(repository, request.UserId, profile.Id);

        LibreTranslateProfile.Apply(profile, request.Url, request.IsDefault);
        profile.LastModifiedBy = request.UserId;
        await repository.UpdateAsync(profile);
        await _unitOfWork.Complete();

        return Result<TranslationProfileResponse>.Success(TranslationProfileMapper.ToResponse(profile));
    }
}
