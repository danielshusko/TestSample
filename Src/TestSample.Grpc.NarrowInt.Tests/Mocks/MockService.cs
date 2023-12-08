namespace TestSample.Grpc.NarrowInt.Tests.Mocks;

public class MockService
{
    public Type ServiceType { get; }
    public object ServiceInstance { get; }

    public MockService(Type serviceType, object serviceInstance)
    {
        if (!serviceType.IsInstanceOfType(serviceInstance))
        {
            throw new Exception("Implementation type must be assignable from service type.");
        }

        ServiceType = serviceType;
        ServiceInstance = serviceInstance;
    }
}