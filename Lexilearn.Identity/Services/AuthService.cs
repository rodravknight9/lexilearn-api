using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lexilearn.Application.Contracts.Identity;
using Lexilearn.Application.Models.Identity;
using Lexilearn.Identity.Models;
using Lexilearn.Identity.Persistence;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Lexilearn.Identity.Services;

public class AuthService : IAuthService
{
    private readonly LexilearnIdentityDbContext _context;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;
    
    public AuthService(LexilearnIdentityDbContext context, IMapper mapper, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _mapper = mapper;
        _jwtSettings = jwtSettings.Value;
    }
    public async Task<AuthResponse> Login(LoginRequest request)
    {
        var user = await _context.Users
            .Where(u => u.UserName == request.UserName)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new Exception("Invalid password");
        }
        
        var response = _mapper.Map<AuthResponse>(user);
        response.Jwt = GenerateJwtToken(user);
        return response;
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName)
            }),
            //Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<AuthResponse> Register(RegistrationRequest request)
    {
        var user = await _context.Users
            .Where(u => u.UserName == request.UserName)
            .FirstOrDefaultAsync();

        if (user != null)
        {
            throw new Exception("User already exists");
        }
        
        var newUser = _mapper.Map<User>(request);
        newUser.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        
        var response = _mapper.Map<AuthResponse>(newUser);
        return response;
    }
}