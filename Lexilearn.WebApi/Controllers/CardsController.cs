using Lexilearn.Application.Features.Lexilearn.Cards.Commands.CreateCard;
using Lexilearn.Application.Features.Lexilearn.Cards.Commands.DeleteCard;
using Lexilearn.Application.Features.Lexilearn.Cards.Commands.EditCard;
using Lexilearn.Application.Features.Lexilearn.Cards.Queries.Common;
using Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetCard;
using Lexilearn.Application.Features.Lexilearn.Cards.Queries.GetCardsByDeck;
using Lexilearn.Shared;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lexilearn.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    public async Task<ActionResult<CreateCardResponse>> Create([FromBody] CreateCardCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPatch]
    public async Task<ActionResult> Update([FromBody] EditCardCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCardResponse>> Get(int id)
    {
        var query = new GetCardQuery(id);
        return Ok(await _mediator.Send(query));
    }

    [HttpGet("Deck/{deckId}/")]
    public async Task<ActionResult<IReadOnlyList<GetCardResponse>>> GetByDeck([FromRoute] int deckId, 
        [FromQuery] PaginationSettings pagination)
    {
        var request = _mapper.Map<GetCardsByDeckQuery>(pagination);
        request.DeckId = deckId;
        return Ok(await _mediator.Send(request));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteCardCommand(id);
        await _mediator.Send(command);
        return Ok();
    }
}