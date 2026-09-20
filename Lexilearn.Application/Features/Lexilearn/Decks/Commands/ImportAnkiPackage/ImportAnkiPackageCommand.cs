using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.ImportAnkiPackage;

public class ImportAnkiPackageCommand : IRequest<Result<ImportAnkiPackageResponse>>
{
    public required Stream FileStream { get; set; }
    public required string TermLanguageCode { get; set; }
    public required string DefinitionLanguageCode { get; set; }
    public int CreatedBy { get; set; }
}
