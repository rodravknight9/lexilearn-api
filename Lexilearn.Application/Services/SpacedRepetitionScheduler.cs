using Lexilearn.Application.Contracts.Services;
using Lexilearn.Domain;
using Lexilearn.Domain.Enums;

namespace Lexilearn.Application.Services;

public class SpacedRepetitionScheduler : ISpacedRepetitionScheduler
{
    private const int HardIntervalDays = 1;
    private const int EasyIntervalDays = 3;
    private const int LearntIntervalDays = 21;

    public void ApplyReview(CardSchedulingState state, ReviewRating rating, DateTime now)
    {
        if (rating == ReviewRating.Fail)
        {
            state.LapseCount++;
            return;
        }

        (state.Status, state.IntervalDays) = rating switch
        {
            ReviewRating.Hard => (SchedulingStatus.Learning, HardIntervalDays),
            ReviewRating.Easy => (SchedulingStatus.Review, EasyIntervalDays),
            ReviewRating.Learnt => (SchedulingStatus.Learnt, LearntIntervalDays),
            _ => (state.Status, state.IntervalDays)
        };

        state.NextReviewAt = now.AddDays(state.IntervalDays);
        state.ReviewCount++;
        state.LastReviewedAt = now;
    }
}
