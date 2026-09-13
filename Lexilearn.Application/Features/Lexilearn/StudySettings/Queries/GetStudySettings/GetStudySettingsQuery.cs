using Lexilearn.Application.Features.Lexilearn.StudySettings.Common;
using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.StudySettings.Queries.GetStudySettings;

public class GetStudySettingsQuery : IRequest<Result<StudySettingsResponse>>
{
    public int DeckId { get; set; }
    public int UserId { get; set; }
}
