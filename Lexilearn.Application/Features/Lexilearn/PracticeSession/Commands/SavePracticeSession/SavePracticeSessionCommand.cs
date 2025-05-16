using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.PracticeSession.Commands.SavePracticeSession;

public class SavePracticeSessionCommand : IRequest<SoftResult>
{
    public int DeckId { get; set; }
    public int CreatedBy { get; set; }
    public ICollection<CardInPractice> Cards { get; set; }
}