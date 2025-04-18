using Lexilearn.Domain.Common;

namespace Lexilearn.Domain;

public class PracticeSessionCards : NonAuditoryBaseDomain
{
    public int Status { get; set; }
    public int SessionId { get; set; }
    public int CardId { get; set; }
    
    public PracticeSession PracticeSession { get; set; }
    public Card Card { get; set; }
}