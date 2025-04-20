using Lexilearn.Application.Models.Identity;

namespace Lexilearn.Application.Contracts.Identity;

public interface IAuthService
{
    Task<AuthResponse> Login(LoginRequest request);
    Task<AuthResponse> Register(RegistrationRequest request);
}