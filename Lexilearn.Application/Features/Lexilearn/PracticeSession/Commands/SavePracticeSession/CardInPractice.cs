using Lexilearn.Domain.Enums;

namespace Lexilearn.Application.Features.Lexilearn.PracticeSession.Commands.SavePracticeSession;

public class CardInPractice
{
    public ReviewRating Rating { get; set; }
    public int SessionId { get; set; }
    public int CardId { get; set; }
}