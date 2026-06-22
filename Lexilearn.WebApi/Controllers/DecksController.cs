using System.Security.Claims;
using Lexilearn.Application.Features.Lexilearn.Decks.Commands.CreateDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Commands.DeleteDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Commands.EditDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.Common;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDeck;
using Lexilearn.Application.Features.Lexilearn.Decks.Queries.GetDecks;
using Lexilearn.Shared;
using Lexilearn.DataTransfer.Decks;
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
            command.CreatedBy = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(command);
            
            if(result.HasErrors)
                return BadRequest(result.Error);
            
            return Ok(result.Value);
        }

        [HttpPatch]
        public async Task<ActionResult> Update([FromBody] EditDeckRequest request)
        {
            var command = _mapper.Map<EditDeckCommand>(request);
            command.LastModifiedBy = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            command.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(command);
            
            if(result.HasErrors)
                return BadRequest(result.Error);
            
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDeckResponse>> GetById(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var query = new GetDeckQuery(id, userId);
            var result = await _mediator.Send(query);
            
            if(result.HasErrors)
                return BadRequest(result.Error);
            
            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<ActionResult<GetDeckResponse>> GetMany([FromQuery] PaginationSettings paginationSettings)
        {
            var query = _mapper.Map<GetDecksQuery>(paginationSettings);
            query.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(query);
            
            if(result.HasErrors)
                return BadRequest(result.Error);
            
            return Ok(result.Value);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var command = new DeleteDeckCommand(id, userId);
            var result = await _mediator.Send(command);
            
            if(result.HasErrors)
                return BadRequest(result.Error);
            
            return NoContent();
        }
    }
}
