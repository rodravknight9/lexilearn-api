using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Cards.Commands.EditCard;

public class EditCardCommand : IRequest<SoftResult>
{
    public int Id { get; set; }
    public string? Front {  get; set; }
    public string? Back {  get; set; }
    public bool? IsFavorite { get; set; }
    public int LastModifiedBy { get; set; }

    public int? DeckId { get; set; }
    public int UserId { get; set; }
}