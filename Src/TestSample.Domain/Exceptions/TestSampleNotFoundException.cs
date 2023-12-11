namespace TestSample.Domain.Exceptions;

public class TestSampleNotFoundException : TestSampleException
{
    public TestSampleNotFoundException(string message)
        : base(message, TestSampleExceptionType.NotFound) { }
}