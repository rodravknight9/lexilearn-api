namespace Lexilearn.Application.Models.LexiLearn;

public class Error
{
    public string Code { get; init; }
    public string Message { get; init; }

    // Identiy errors
    public static Error UserNotFound => new() { Code = "USR-001", Message = "User not found" };
    public static Error InvalidPassword => new() { Code = "USR-002", Message = "Password is invalid" };
    public static Error UserAlreadyExists => new() { Code = "USR-003", Message = "User already exists" };

    public static Error NotFound => new() { Code = "RES-001", Message = "Resource not found" };
    public static Error Forbidden => new() { Code = "RES-002", Message = "You do not have access to this resource" };
}