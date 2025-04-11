using Lexilearn.Application.Features.Lexilearn.Decks.Commands.CreateDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDecks;
using Lexilearn.Shared;
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

        [HttpPost]
        public async Task<ActionResult<CreateDeckResponse>> Create([FromBody] CreateDeckCommand command)
        { 
            return Ok(await _mediator.Send(command));
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
    }
}
