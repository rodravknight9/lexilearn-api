using Lexilearn.Domain.Common;

namespace Lexilearn.Domain;

public class StudySessionSettings : NonAuditoryBaseDomain
{
    public int DeckId { get; set; }

    public int SessionSize { get; set; } = 20;
    public int NewCardsPercentage { get; set; } = 20;
    public int HardCardsPercentage { get; set; } = 30;
}
