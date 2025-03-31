using Lexilearn.Domain.Common;

namespace Lexilearn.Domain
{
    public class Deck : BaseDomainModel
    {
        public required string Title { get; set; }
        public required string TermLanguageCode { get; set; }
        public required string DefinitionLanguageCode { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }
}
