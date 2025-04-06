using Lexilearn.Application.Features.Lexilearn.Decks.Commands.CreateDeck;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lexilearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DeckController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        public async Task<ActionResult<int>> Create([FromBody] CreateDeckCommand command)
        { 
            return await _mediator.Send(command);
        }
    }
}
