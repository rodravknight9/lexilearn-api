namespace Lexilearn.Application.Models.AnkiImport
{
    public class AnkiPackage
    {
        public List<AnkiDeck> Decks { get; set; } = new();
        public int SkippedAttachmentCount { get; set; }
    }

    public class AnkiDeck
    {
        public required string Name { get; set; }
        public List<AnkiNote> Notes { get; set; } = new();
    }

    public class AnkiNote
    {
        public required string CardExternalId { get; set; }
        public required string Front { get; set; }
        public required string Back { get; set; }
    }
}
