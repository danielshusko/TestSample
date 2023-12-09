using TestSample.Domain.Exceptions;

namespace TestSample.Domain;

public class Result<TValue> : Result<TValue, TestSampleException>
{
    public Result(TValue value) : base(value) { }

    public Result(TestSampleException error) : base(error) { }

    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static implicit operator Result<TValue>(TestSampleException error) => new(error);
}

public class Result<TValue, TException>
{
    public readonly TException? Error;
    public readonly TValue? Value;

    public bool IsSuccess { get; }

    public Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
        Error = default;
    }

    public Result(TException error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    public static implicit operator Result<TValue, TException>(TValue value) => new(value);

    public static implicit operator Result<TValue, TException>(TException error) => new(error);
}