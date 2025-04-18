using Lexilearn.Domain.Common;

namespace Lexilearn.Domain;

public class PracticeSession : NonAuditoryBaseDomain
{
    public int UserId { get; set; }
    public int DeckId { get; set; }

    public Deck Deck { get; set; }
    public ICollection<PracticeSessionCards> Cards { get; set; }
}