using Lexilearn.Application.Features.Translation.Profiles.Common;
using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Profiles.Queries.GetTranslationProfiles;

public class GetTranslationProfilesQuery : IRequest<Result<IReadOnlyList<TranslationProfileResponse>>>
{
    public int UserId { get; set; }
}
