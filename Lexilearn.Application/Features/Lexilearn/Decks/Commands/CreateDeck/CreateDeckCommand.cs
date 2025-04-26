using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.CreateDeck
{
    public class CreateDeckCommand : IRequest<Result<CreateDeckResponse>>
    {
        public string Title { get; set; }
        public string TermLanguageCode { get; set; }
        public string DefinitionLanguageCode { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public string CreatedBy { get; set; }
    }
}
