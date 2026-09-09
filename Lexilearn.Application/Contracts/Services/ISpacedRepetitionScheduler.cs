using Lexilearn.Domain;
using Lexilearn.Domain.Enums;

namespace Lexilearn.Application.Contracts.Services;

public interface ISpacedRepetitionScheduler
{
    void ApplyReview(CardSchedulingState state, ReviewRating rating, DateTime now);
}
