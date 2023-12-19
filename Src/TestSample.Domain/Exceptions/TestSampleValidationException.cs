namespace TestSample.Domain.Exceptions;

public class TestSampleValidationException : TestSampleException
{
    public TestSampleValidationException(string message)
        : base(message, TestSampleExceptionType.Validation) { }
}