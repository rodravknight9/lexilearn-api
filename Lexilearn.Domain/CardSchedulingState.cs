using Lexilearn.Domain.Common;
using Lexilearn.Domain.Enums;

namespace Lexilearn.Domain;

public class CardSchedulingState : NonAuditoryBaseDomain
{
    public int CardId { get; set; }
    public Card Card { get; set; } = null!;

    public SchedulingStatus Status { get; set; } = SchedulingStatus.New;
    public DateTime? NextReviewAt { get; set; }
    public DateTime? LastReviewedAt { get; set; }

    public int ReviewCount { get; set; }
    public int LapseCount { get; set; }
    public int IntervalDays { get; set; }

    public DateTime? LastModifiedDate { get; set; }
}
