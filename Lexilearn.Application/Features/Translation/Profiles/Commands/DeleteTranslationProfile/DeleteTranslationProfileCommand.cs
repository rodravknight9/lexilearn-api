using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Profiles.Commands.DeleteTranslationProfile;

public class DeleteTranslationProfileCommand : IRequest<SoftResult>
{
    public int Id { get; }
    public int UserId { get; }

    public DeleteTranslationProfileCommand(int id, int userId)
    {
        Id = id;
        UserId = userId;
    }
}
