using Lexilearn.Application.Features.Lexilearn.Decks.Commands.CreateDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Commands.DeleteDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Commands.EditDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDecks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexilearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DeckController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DeckController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CreateDeckResponse>> Create([FromBody] CreateDeckCommand command)
        { 
            return Ok(await _mediator.Send(command));
        }

        [HttpPatch]
        public async Task<ActionResult> Update([FromBody] EditDeckCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDeckResponse>> GetById(int id)
        {
            var query = new GetDeckQuery(id);
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        public async Task<ActionResult<GetDeckResponse>> GetMany([FromQuery] GetDecksQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteDeckCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
