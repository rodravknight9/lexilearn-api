using Lexilearn.Application.Contracts.Infastructure;
using Lexilearn.Application.Contracts.Persistence;
using Lexilearn.Application.Models.AnkiImport;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain;
using MediatR;

namespace Lexilearn.Application.Features.Lexilearn.Decks.Commands.ImportAnkiPackage;

public class ImportAnkiPackageCommandHandler : IRequestHandler<ImportAnkiPackageCommand, Result<ImportAnkiPackageResponse>>
{
    private const string AnkiImportSource = "anki";

    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnkiPackageParser _parser;

    public ImportAnkiPackageCommandHandler(IUnitOfWork unitOfWork, IAnkiPackageParser parser)
    {
        _unitOfWork = unitOfWork;
        _parser = parser;
    }

    public async Task<Result<ImportAnkiPackageResponse>> Handle(ImportAnkiPackageCommand request, CancellationToken cancellationToken)
    {
        AnkiPackage package;
        try
        {
            package = await _parser.ParseAsync(request.FileStream, cancellationToken);
        }
        catch (AnkiImportException)
        {
            return Result<ImportAnkiPackageResponse>.Failure(Error.InvalidAnkiPackage);
        }

        if (package.Decks.Count == 0)
            return Result<ImportAnkiPackageResponse>.Failure(Error.EmptyAnkiPackage);

        // Deliberately not filtered by IsActive: Card(ImportSource, ExternalId) is a hard unique
        // index at the database level that doesn't know about soft-deletes either, so a deleted
        // card's ExternalId still can't be reused. Treating it as "available again" here would
        // just move the conflict from a clean skip to an unhandled unique-constraint violation.
        var existingExternalIds = (await _unitOfWork.Repository<Card>()
                .GetMany(c => c.ImportSource == AnkiImportSource && c.Deck.CreatedBy == request.CreatedBy))
            .Select(c => c.ExternalId)
            .Where(id => id is not null)
            .ToHashSet();

        var response = new ImportAnkiPackageResponse { SkippedAttachments = package.SkippedAttachmentCount };
        var createdDecks = new List<(Deck Entity, int ImportedCount)>();

        foreach (var ankiDeck in package.Decks)
        {
            var newNotes = new List<AnkiNote>();
            foreach (var note in ankiDeck.Notes)
            {
                if (existingExternalIds.Contains(note.CardExternalId))
                {
                    response.TotalCardsSkipped++;
                }
                else
                {
                    newNotes.Add(note);
                }
            }

            // Every note in this Anki deck was already imported before: skip creating an
            // otherwise-empty duplicate deck for it.
            if (newNotes.Count == 0)
                continue;

            var deckEntity = new Deck
            {
                Title = ankiDeck.Name,
                TermLanguageCode = request.TermLanguageCode,
                DefinitionLanguageCode = request.DefinitionLanguageCode,
                CreatedBy = request.CreatedBy,
            };
            await _unitOfWork.Repository<Deck>().AddAsync(deckEntity);

            foreach (var note in newNotes)
            {
                var cardEntity = new Card
                {
                    Front = note.Front,
                    Back = note.Back,
                    ImportSource = AnkiImportSource,
                    ExternalId = note.CardExternalId,
                    Deck = deckEntity,
                    SchedulingState = new CardSchedulingState(),
                };
                await _unitOfWork.Repository<Card>().AddAsync(cardEntity);
            }

            createdDecks.Add((deckEntity, newNotes.Count));
            response.TotalCardsImported += newNotes.Count;
        }

        await _unitOfWork.Complete();

        response.Decks = createdDecks
            .Select(d => new ImportedDeckSummary { DeckId = d.Entity.Id, Title = d.Entity.Title, CardCount = d.ImportedCount })
            .ToList();

        return Result<ImportAnkiPackageResponse>.Success(response);
    }
}
