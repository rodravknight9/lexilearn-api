using Lexilearn.Domain.Common;

namespace Lexilearn.Domain
{
    public class Card : AuditoryBaseDomain
    {
        public required string Front {  get; set; }
        public required string Back {  get; set; }
        public string? Example { get; set; }
        public bool IsFavorite { get; set; }

        public string? ImportSource { get; set; }
        public string? ExternalId { get; set; }

        public int DeckId { get; set; }
        public Deck Deck { get; set; }

        public CardSchedulingState SchedulingState { get; set; } = null!;
    }
}
