using Lexilearn.Application.Contracts.Infastructure;
using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Models.CustomTranslate;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Application.Models.LibreTranslate;
using Lexilearn.Domain;
using Lexilearn.Domain.Enums;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Commands.TranslateText
{
    public class TranslationHandler : IRequestHandler<TranslateTextCommand, Result<TranslationResponse>>
    {
        private readonly ITranslationService _translationService;
        private readonly ICustomTranslationService _customTranslationService;
        private readonly IUnitOfWork _unitOfWork;

        public TranslationHandler(
            ITranslationService translationService,
            ICustomTranslationService customTranslationService,
            IUnitOfWork unitOfWork)
        {
            _translationService = translationService;
            _customTranslationService = customTranslationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<TranslationResponse>> Handle(TranslateTextCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await Translate(request, cancellationToken);
            }
            catch (InvalidOperationException ex)
            {
                return Result<TranslationResponse>.Failure(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return Result<TranslationResponse>.Failure(ex.Message);
            }
        }

        private async Task<Result<TranslationResponse>> Translate(TranslateTextCommand request, CancellationToken cancellationToken)
        {
            var profileId = request.TranslationProfileId is int id && id > 0 ? id : (int?)null;
            var profile = await ResolveProfile(request, profileId);
            if (profile is null)
            {
                return profileId.HasValue
                    ? Result<TranslationResponse>.Failure($"{Error.NotFound.Code}: Translation profile was not found.")
                    : Result<TranslationResponse>.Failure("Choose a translation profile before translating.");
            }

            if (profile.Kind == TranslationProfileKind.LibreTranslate)
            {
                var response = await _translationService.TranslateText(
                    new TranslationRequest
                    {
                        q = request.Text,
                        source = request.LanguageSourceCode,
                        target = request.LanguageTargetCode
                    },
                    profile.Url);

                return Result<TranslationResponse>.Success(response);
            }

            var translated = await _customTranslationService.TranslateAsync(new CustomTranslationCall
            {
                Url = profile.Url,
                HttpMethod = profile.HttpMethod,
                Headers = RequestEntryJson.Deserialize(profile.HeadersJson),
                Body = RequestEntryJson.Deserialize(profile.BodyJson),
                ResponsePath = profile.ResponsePath,
                Text = request.Text,
                Source = request.LanguageSourceCode,
                Target = request.LanguageTargetCode
            }, cancellationToken);

            return Result<TranslationResponse>.Success(translated);
        }

        private async Task<TranslationProfile?> ResolveProfile(TranslateTextCommand request, int? profileId)
        {
            var repository = _unitOfWork.Repository<TranslationProfile>();
            if (profileId is int id)
            {
                return (await repository.GetMany(p =>
                        p.Id == id && p.UserId == request.UserId && p.IsActive))
                    .FirstOrDefault();
            }

            return (await repository.GetMany(p =>
                    p.UserId == request.UserId && p.IsDefault && p.IsActive))
                .FirstOrDefault();
        }
    }
}
