using Lexilearn.Application.Features.Translation.Commands.TranslateText;
using Lexilearn.Application.Models.LexiLearn;
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

        [HttpPost("Translate")]
        public async Task<ActionResult<TranslationResponse>> Translate([FromBody] TranslateTextCommand command)
        { 
            return await _mediator.Send(command);
        }
    }
}
