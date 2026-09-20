using Microsoft.AspNetCore.Http;

namespace Lexilearn.DataTransfer.Decks;

public class ImportAnkiPackageRequest
{
    public required IFormFile File { get; set; }
    public required string TermLanguageCode { get; set; }
    public required string DefinitionLanguageCode { get; set; }
}
