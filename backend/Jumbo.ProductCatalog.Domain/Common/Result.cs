namespace Jumbo.ProductCatalog.Domain.Common;

/// <summary>
/// Non-generic result for void operations, and factory for typed results.
/// Static members live here so Result&lt;T&gt; stays CA1000-clean.
/// </summary>
public sealed class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }

    private Result(bool success, string? error) { IsSuccess = success; Error = error; }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);

    // Typed factories — static members on a non-generic class, satisfies CA1000.
    public static Result<T> Success<T>(T value) => new(value);
    public static Result<T> Failure<T>(string error) => new(error);
}

public sealed class Result<T>
{
    private readonly T? _value;

    public bool IsSuccess { get; }
    public string? Error { get; }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access Value on a failed Result.");

    // Internal: consumers use the Result factory class.
    internal Result(T value) { IsSuccess = true; _value = value; }
    internal Result(string error) { IsSuccess = false; Error = error; }
}
