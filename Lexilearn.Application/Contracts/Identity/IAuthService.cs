using Lexilearn.Application.Models.Identity;
using Lexilearn.Application.Models.LexiLearn;

namespace Lexilearn.Application.Contracts.Identity;

public interface IAuthService
{
    Task<Result<AuthResponse>> Login(LoginRequest request);
    Task<Result<AuthResponse>> Register(RegistrationRequest request);
}