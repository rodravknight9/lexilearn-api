using Lexilearn.Application.Contracts.Identity;
using Lexilearn.Application.Models.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = Lexilearn.Application.Models.Identity.LoginRequest;

namespace Lexilearn.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService; 
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest loginRequest)
    {
        var response = await _authService.Login(loginRequest);
        if (response.HasErrors)
            return BadRequest(response.Error);
        
        return Ok(response.Value);
    }
    
    [HttpPost("Register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegistrationRequest registerRequest)
    {
        var response = await _authService.Register(registerRequest);
        if (response.HasErrors)
            return BadRequest(response.Error);
        
        return Ok(response.Value);
    }
}