using Newtonsoft.Json.Linq;

namespace cafedebug_backend.domain.Shared;
public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; private set; }
    
    protected Result(bool isSuccess, string error)
    {
        if (isSuccess && error != string.Empty)
            throw new InvalidOperationException();

        if (!isSuccess && error == string.Empty)
            throw new InvalidOperationException("A failure must have an error message.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, string.Empty);

    public static Result Failure(string error) => new(false, error);
}

public class Result<TValue> : Result
{
    public TValue Value { get; private set; }

    protected internal Result(TValue value, bool isSuccess, string error) : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<TValue> Success(TValue value) => new(value, true, string.Empty);
    public static Result<TValue> Success() => new(default(TValue), true, string.Empty);
    public static Result<TValue> Failure(string error) => new(default(TValue), false, error);
}