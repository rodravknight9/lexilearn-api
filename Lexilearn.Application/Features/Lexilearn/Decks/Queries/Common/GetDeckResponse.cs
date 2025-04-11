namespace Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common
{
    public class GetDeckResponse
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public required string Title { get; set; }
        public required string TermLanguageCode { get; set; }
        public required string DefinitionLanguageCode { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }
}
