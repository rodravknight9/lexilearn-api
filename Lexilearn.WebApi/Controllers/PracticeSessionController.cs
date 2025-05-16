using Lexilearn.Application.Features.Lexilearn.PracticeSession.Commands.SavePracticeSession;
using Lexilearn.WebApi.DataTransfer.PracticeSessions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Lexilearn.Application.Features.Lexilearn.PracticeSession.Queries.GetSessionHistory;

namespace Lexilearn.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PracticeSessionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public PracticeSessionController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] SavePracticeSessionRequest request)
    {
        var command = _mapper.Map<SavePracticeSessionCommand>(request);
        command.CreatedBy = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); 
        var result = await _mediator.Send(command);
            
        if(result.HasErrors)
            return BadRequest(result.Error);
            
        return NoContent();
    }

    [HttpGet("StartDate/{startDate}/EndDate/{endDate}")]
    public async Task<ActionResult<GetSessionHistoryResponse>> Get(DateTime startDate, DateTime endDate)
    {
        GetSessionHistoryQuery query = new(){
            StartDate = startDate,
            EndDate = endDate
        };
        
        var result = await _mediator.Send(query);
        
        if(result.HasErrors)
            return BadRequest(result.Error);
            
        return Ok(result.Value);
    }
}