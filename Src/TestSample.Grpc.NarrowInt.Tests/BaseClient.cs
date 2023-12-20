using TestSample.Domain;

namespace TestSample.Grpc.NarrowInt.Tests;

public class BaseClient
{
    /// <summary>
    /// This method is an example. The real implementation should use a tenant id
    /// set at the test level.
    /// </summary>
    /// <returns></returns>
    protected static Dictionary<string, string> GetRequestHeaders() =>
        new()
        {
            { Constants.TenantIdKey, Guid.NewGuid().ToString() }
        };
}