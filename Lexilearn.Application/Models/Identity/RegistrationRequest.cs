namespace Lexilearn.Application.Models.Identity;

public class RegistrationRequest
{
    public required string UserName { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public required string Password { get; set; }
}