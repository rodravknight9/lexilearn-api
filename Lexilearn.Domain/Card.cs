using Lexilearn.Domain.Common;

namespace Lexilearn.Domain
{
    public class Card : BaseDomainModel
    {
        public required string Front {  get; set; }
        public required string Back {  get; set; }
        public bool IsFavorite { get; set; }
    }
}
