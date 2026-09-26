using System.Text.Json.Serialization;
using Lexilearn.Application.Features.Translation.Profiles.Common;
using Lexilearn.Application.Models.CustomTranslate;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.Domain.Enums;
using MediatR;

namespace Lexilearn.Application.Features.Translation.Profiles.Commands.CreateTranslationProfile;

public class CreateTranslationProfileCommand : IRequest<Result<TranslationProfileResponse>>
{
    [JsonIgnore]
    public int UserId { get; set; }

    public string Name { get; set; } = null!;
    public TranslationProfileKind Kind { get; set; } = TranslationProfileKind.Custom;
    public bool IsDefault { get; set; }
    public string Url { get; set; } = null!;
    public string HttpMethod { get; set; } = "POST";
    public List<RequestEntry> Headers { get; set; } = [];
    public List<RequestEntry> Body { get; set; } = [];
    public string ResponsePath { get; set; } = null!;
}
