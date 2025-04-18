using Lexilearn.Domain.Common;

namespace Lexilearn.Domain
{
    public class Card : AuditoryBaseDomain
    {
        public required string Front {  get; set; }
        public required string Back {  get; set; }
        public bool IsFavorite { get; set; }

        public int DeckId { get; set; }
        public Deck Deck { get; set; }
    }
}
