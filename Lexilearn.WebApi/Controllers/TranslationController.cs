using Lexilearn.Application.Features.Translation.Commands.TranslateText;
using Lexilearn.Application.Models.LibreTranslate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lexilearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TranslationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        public async Task<ActionResult<TranslationResponse>> Translate([FromBody] TranslateTextCommand command)
        { 
            return Ok(await _mediator.Send(command));
        }
    }
}
