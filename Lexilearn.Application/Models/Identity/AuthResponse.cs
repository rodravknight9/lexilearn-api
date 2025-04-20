namespace Lexilearn.Application.Models.Identity;

public class AuthResponse
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Jwt { get; set; }
}