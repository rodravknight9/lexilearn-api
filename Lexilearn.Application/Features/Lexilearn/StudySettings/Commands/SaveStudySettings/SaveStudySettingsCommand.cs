using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.StudySettings.Commands.SaveStudySettings;

public class SaveStudySettingsCommand : IRequest<SoftResult>
{
    public int DeckId { get; set; }
    public int UserId { get; set; }

    public int SessionSize { get; set; }
    public int NewCardsPercentage { get; set; }
    public int HardCardsPercentage { get; set; }
}
