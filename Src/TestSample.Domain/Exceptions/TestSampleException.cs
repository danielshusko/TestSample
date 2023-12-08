namespace TestSample.Domain.Exceptions;
public class TestSampleException : Exception
{
    public TestSampleException(string message)
    : base(message) { }
}