using System.Text.Json.Serialization;
using Lexilearn.Application.Features.Translation.Profiles.Common;
using Lexilearn.Application.Models.CustomTranslate;
using Lexilearn.Application.Models.LexiLearn;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Profiles.Commands.UpdateTranslationProfile;

public class UpdateTranslationProfileCommand : IRequest<Result<TranslationProfileResponse>>
{
    [JsonIgnore]
    public int Id { get; set; }

    [JsonIgnore]
    public int UserId { get; set; }

    public string Name { get; set; } = null!;
    public bool IsDefault { get; set; }
    public string Url { get; set; } = null!;
    public string HttpMethod { get; set; } = "POST";
    public List<RequestEntry> Headers { get; set; } = [];
    public List<RequestEntry> Body { get; set; } = [];
    public string ResponsePath { get; set; } = null!;
}
