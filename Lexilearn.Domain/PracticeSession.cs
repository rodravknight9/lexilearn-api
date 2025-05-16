using Lexilearn.Domain.Common;

namespace Lexilearn.Domain;

public class PracticeSession : NonAuditoryBaseDomain
{
    public int DeckId { get; set; }

    public Deck Deck { get; set; }
    public ICollection<PracticeSessionCards> Cards { get; set; }
}