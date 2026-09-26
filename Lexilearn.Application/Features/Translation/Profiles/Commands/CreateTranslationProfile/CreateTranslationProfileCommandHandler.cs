using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Features.Translation.Profiles.Common;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using Lexilearn.Domain.Enums;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Profiles.Commands.CreateTranslationProfile;

public class CreateTranslationProfileCommandHandler
    : IRequestHandler<CreateTranslationProfileCommand, Result<TranslationProfileResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTranslationProfileCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TranslationProfileResponse>> Handle(
        CreateTranslationProfileCommand request,
        CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<TranslationProfile>();
        if (request.Kind == TranslationProfileKind.LibreTranslate)
            return await CreateLibreTranslate(request, repository);

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
            await TranslationProfileDefaults.ClearAsync(repository, request.UserId, exceptId: null);

        var profile = new TranslationProfile
        {
            UserId = request.UserId,
            CreatedBy = request.UserId,
            Kind = TranslationProfileKind.Custom
        };
        TranslationProfileMapper.Apply(profile, validated, request.IsDefault);
        await repository.AddAsync(profile);
        await _unitOfWork.Complete();

        return Result<TranslationProfileResponse>.Success(TranslationProfileMapper.ToResponse(profile));
    }

    private async Task<Result<TranslationProfileResponse>> CreateLibreTranslate(
        CreateTranslationProfileCommand request,
        IAsyncRepository<TranslationProfile> repository)
    {
        var urlError = LibreTranslateProfile.ValidateUrl(request.Url);
        if (urlError is not null)
            return Result<TranslationProfileResponse>.Failure(urlError);

        var existing = (await repository.GetMany(p =>
                p.UserId == request.UserId
                && p.Kind == TranslationProfileKind.LibreTranslate
                && p.IsActive))
            .FirstOrDefault();
        if (existing is not null)
            return Result<TranslationProfileResponse>.Failure("A LibreTranslate profile already exists.");

        if (request.IsDefault)
            await TranslationProfileDefaults.ClearAsync(repository, request.UserId, exceptId: null);

        var profile = new TranslationProfile
        {
            UserId = request.UserId,
            CreatedBy = request.UserId
        };
        LibreTranslateProfile.Apply(profile, request.Url, request.IsDefault);
        await repository.AddAsync(profile);
        await _unitOfWork.Complete();

        return Result<TranslationProfileResponse>.Success(TranslationProfileMapper.ToResponse(profile));
    }
}
