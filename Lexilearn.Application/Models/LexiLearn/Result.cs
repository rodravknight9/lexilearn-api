namespace Lexilearn.Application.Models.LexiLearn;

public class Result<T>
{
    public bool HasErrors { get; set; }
    public string? Error { get; set; }
    public T? Value { get; set; }

    public static Result<T> Success(T value) => new() { HasErrors = false, Value = value };
    public static Result<T> Failure(string error) => new() { HasErrors = true, Error = error };
    public static Result<T> Failure(Error error) => new() { HasErrors = true, Error = $"{error.Code}: {error.Message}" };
}