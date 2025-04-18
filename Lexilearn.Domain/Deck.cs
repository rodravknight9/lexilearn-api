using Lexilearn.Domain.Common;

namespace Lexilearn.Domain
{
    public class Deck : AuditoryBaseDomain
    {
        public required string Title { get; set; }
        public required string TermLanguageCode { get; set; }
        public required string DefinitionLanguageCode { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }

        public ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
