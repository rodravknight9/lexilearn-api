using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.EditDeck;

public class EditDeckCommand : IRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string TermLanguageCode { get; set; }
    public string DefinitionLanguageCode { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
}