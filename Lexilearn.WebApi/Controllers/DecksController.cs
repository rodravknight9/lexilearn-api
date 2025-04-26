using System.Security.Claims;
using Lexilearn.Application.Features.Lexilearn.Decks.Commands.CreateDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Commands.DeleteDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Commands.EditDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDecks;
using Lexilearn.Shared;
using Lexilearn.WebApi.DataTransfer.Decks;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexilearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DecksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public DecksController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<CreateDeckResponse>> Create([FromBody] CreateDeckRequest request)
        { 
            var command = _mapper.Map<CreateDeckCommand>(request);
            command.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await _mediator.Send(command));
        }

        [HttpPatch]
        public async Task<ActionResult> Update([FromBody] EditDeckRequest request)
        {
            var command = _mapper.Map<EditDeckCommand>(request);
            command.LastModifiedBy = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); 
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDeckResponse>> GetById(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var query = new GetDeckQuery(id, userId);
            return Ok(await _mediator.Send(query));
        }

        [HttpGet]
        public async Task<ActionResult<GetDeckResponse>> GetMany([FromQuery] PaginationSettings paginationSettings)
        {
            var query = _mapper.Map<GetDecksQuery>(paginationSettings);
            query.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(await _mediator.Send(query));
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteDeckCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
