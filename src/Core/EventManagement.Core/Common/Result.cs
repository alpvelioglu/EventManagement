namespace EventManagement.Core.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string? error = null)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException();
        if (!isSuccess && error == null)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true);
    public static Result Failure(string error) => new(false, error);
}

public class Result<T> : Result
{
    private readonly T? _value;

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access Value of failed result");

    protected internal Result(T? value, bool isSuccess, string? error = null)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public static new Result<T> Success(T value) => new(value, true);
    public static new Result<T> Failure(string error) => new(default, false, error);
}
