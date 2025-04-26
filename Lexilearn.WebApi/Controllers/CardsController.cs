using System.Security.Claims;
using Lexilearn.Application.Features.Lexilearn.Cards.Commands.CreateCard;
using Lexilearn.Application.Features.Lexilearn.Cards.Commands.DeleteCard;
using Lexilearn.Application.Features.Lexilearn.Cards.Commands.EditCard;
using Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;
using Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetCard;
using Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetCardsByDeck;
using Lexilearn.Shared;
using Lexilearn.WebApi.DataTransfer.Cards;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexilearn.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CardsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CardsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<CreateCardResponse>> Create([FromBody] CreateCardRequest request)
    {
        var command = _mapper.Map<CreateCardCommand>(request);
        command.CreatedBy = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); 
        var result = await _mediator.Send(command);
            
        if(result.HasErrors)
            return BadRequest(result.Error);
            
        return Ok(result.Value);
    }

    [HttpPatch]
    public async Task<ActionResult> Update([FromBody] EditCardRequest request)
    {
        var command = _mapper.Map<EditCardCommand>(request);
        command.LastModifiedBy = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); 
        var result = await _mediator.Send(command);
            
        if(result.HasErrors)
            return BadRequest(result.Error);
            
        return NoContent();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCardResponse>> Get(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetCardQuery(id, userId);
        var result = await _mediator.Send(query);
            
        if(result.HasErrors)
            return BadRequest(result.Error);
            
        return Ok(result.Value);
    }

    [HttpGet("Deck/{deckId}/")]
    public async Task<ActionResult<IReadOnlyList<GetCardResponse>>> GetByDeck([FromRoute] int deckId, 
        [FromQuery] PaginationSettings pagination)
    {
        var request = _mapper.Map<GetCardsByDeckQuery>(pagination);
        request.DeckId = deckId;

        var result = await _mediator.Send(request); 
        
        if(result.HasErrors)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new DeleteCardCommand(id, userId);
        var result = await _mediator.Send(command);
            
        if(result.HasErrors)
            return BadRequest(result.Error);
            
        return NoContent();
    }
}