using TestSample.Domain.Exceptions;

namespace TestSample.Domain;

public class Result<TValue>
{
    public readonly TestSampleException? Error;
    public readonly TValue? Value;

    public bool IsSuccess { get; }

    public Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
        Error = default;
    }

    public Result(TestSampleException error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    //happy path
    public static implicit operator Result<TValue>(TValue value) => new(value);

    //error path
    public static implicit operator Result<TValue>(TestSampleException error) => new(error);

    public Result<TValue> Match(Func<TValue, Result<TValue>> success, Func<TestSampleException, Result<TValue>> failure) =>
        IsSuccess ? success(Value!) : failure(Error!);
}