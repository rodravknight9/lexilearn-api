namespace Lexilearn.WebApi.DataTransfer.PracticeSessions;

public class SavePracticeSessionRequest
{
    public int DeckId { get; set; }
    public int CreatedBy { get; set; }
    public ICollection<SavePracticeSessionCards> Cards { get; set; }
}