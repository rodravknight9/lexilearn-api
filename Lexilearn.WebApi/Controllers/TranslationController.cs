using System.Security.Claims;
using Lexilearn.Application.Features.Translation.Commands.TranslateText;
using Lexilearn.Application.Models.LexiLearn;
using Lexilearn.DataTransfer.Translation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexilearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TranslationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TranslationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        public async Task<ActionResult<TranslationOutput>> Translate([FromBody] TranslateTextCommand command)
        {
            command.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var response = await _mediator.Send(command);
            if (response.HasErrors)
                return TranslateError(response.Error!);

            var result = new TranslationOutput()
            {
                TranslatedText = response.Value!.translatedText
            };
            return Ok(result);
        }

        private ActionResult TranslateError(string error) =>
            error.StartsWith(Error.NotFound.Code, StringComparison.Ordinal)
                ? NotFound(error)
                : BadRequest(error);
    }
}
