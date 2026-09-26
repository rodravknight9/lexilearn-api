using System.Security.Claims;
using Lexilearn.Application.Features.Translation.Profiles.Commands.CreateTranslationProfile;
using Lexilearn.Application.Features.Translation.Profiles.Commands.DeleteTranslationProfile;
using Lexilearn.Application.Features.Translation.Profiles.Commands.UpdateTranslationProfile;
using Lexilearn.Application.Features.Translation.Profiles.Common;
using Lexilearn.Application.Features.Translation.Profiles.Queries.GetTranslationProfile;
using Lexilearn.Application.Features.Translation.Profiles.Queries.GetTranslationProfiles;
using Lexilearn.Application.Models.LexiLearn;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexilearn.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TranslationProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TranslationProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TranslationProfileResponse>>> GetMany()
    {
        var result = await _mediator.Send(new GetTranslationProfilesQuery { UserId = CurrentUserId() });
        if (result.HasErrors)
            return FromError(result.Error!);

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TranslationProfileResponse>> GetById(int id)
    {
        var result = await _mediator.Send(new GetTranslationProfileQuery
        {
            Id = id,
            UserId = CurrentUserId()
        });
        if (result.HasErrors)
            return FromError(result.Error!);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<TranslationProfileResponse>> Create([FromBody] CreateTranslationProfileCommand command)
    {
        command.UserId = CurrentUserId();
        var result = await _mediator.Send(command);
        if (result.HasErrors)
            return FromError(result.Error!);

        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TranslationProfileResponse>> Update(int id, [FromBody] UpdateTranslationProfileCommand command)
    {
        command.Id = id;
        command.UserId = CurrentUserId();
        var result = await _mediator.Send(command);
        if (result.HasErrors)
            return FromError(result.Error!);

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteTranslationProfileCommand(id, CurrentUserId()));
        if (result.HasErrors)
            return FromError(result.Error);

        return NoContent();
    }

    private int CurrentUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private ActionResult FromError(string error) =>
        error.StartsWith(Error.NotFound.Code, StringComparison.Ordinal)
            ? NotFound(error)
            : BadRequest(error);
}
