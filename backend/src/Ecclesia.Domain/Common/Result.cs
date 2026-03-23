namespace Ecclesia.Domain.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public IEnumerable<string> Errors { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Errors = Enumerable.Empty<string>();
    }

    private Result(IEnumerable<string> errors)
    {
        IsSuccess = false;
        Value = default;
        Errors = errors;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(IEnumerable<string> errors) => new(errors);
    public static Result<T> Failure(string error) => new(new[] { error });
}