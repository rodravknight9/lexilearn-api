using Lexilearn.Application.Features.Translation.Profiles.Common;
using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Profiles.Queries.GetTranslationProfile;

public class GetTranslationProfileQuery : IRequest<Result<TranslationProfileResponse>>
{
    public int Id { get; set; }
    public int UserId { get; set; }
}
