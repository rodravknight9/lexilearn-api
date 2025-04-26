namespace Lexilearn.Application.Models.LexiLearn;

public class SoftResult
{
    public string Error { get; }
    public bool HasErrors;

    protected SoftResult(bool hasErrors, string error)
    {
        HasErrors = hasErrors;
        Error = error;
    }

    public static SoftResult Success() => new SoftResult(false, string.Empty);
    public static SoftResult Failure(string error) => new SoftResult(true, error);
    public static SoftResult Failure(Error error) => new SoftResult(true, $"{error.Code}: {error.Message}");

}