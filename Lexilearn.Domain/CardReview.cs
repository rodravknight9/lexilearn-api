using Lexilearn.Domain.Common;
using Lexilearn.Domain.Enums;

namespace Lexilearn.Domain;

public class CardReview : NonAuditoryBaseDomain
{
    public int SessionId { get; set; }
    public int CardId { get; set; }

    public ReviewRating Rating { get; set; }
    public DateTime ReviewedAt { get; set; }

    public SchedulingStatus PreviousStatus { get; set; }
    public SchedulingStatus NextStatus { get; set; }
    public DateTime? PreviousReviewAt { get; set; }
    public DateTime? NextReviewAt { get; set; }
    public int? ResponseTimeMilliseconds { get; set; }

    public PracticeSession PracticeSession { get; set; }
    public Card Card { get; set; }
}
