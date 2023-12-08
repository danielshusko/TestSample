namespace TestSample.Domain.Exceptions;

public class TestSampleException : Exception
{
    public TestSampleExceptionType Type { get; }

    public TestSampleException(string message, TestSampleExceptionType type)
        : base(message)
    {
        Type = type;
    }
}