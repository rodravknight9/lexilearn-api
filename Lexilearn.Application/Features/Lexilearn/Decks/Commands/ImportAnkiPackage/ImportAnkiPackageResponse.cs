namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.ImportAnkiPackage;

public class ImportAnkiPackageResponse
{
    public List<ImportedDeckSummary> Decks { get; set; } = new();
    public int TotalCardsImported { get; set; }
    public int TotalCardsSkipped { get; set; }
    public int SkippedAttachments { get; set; }
}

public class ImportedDeckSummary
{
    public int DeckId { get; set; }
    public required string Title { get; set; }
    public int CardCount { get; set; }
}
