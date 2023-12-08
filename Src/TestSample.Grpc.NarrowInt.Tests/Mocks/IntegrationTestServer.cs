using Grpc.Net.Client;
using TestSample.Api;

namespace TestSample.Grpc.NarrowInt.Tests.Mocks;

public class IntegrationTestServer : IDisposable
{
    public GrpcChannel Channel { get; }
    public HttpClient HttpClient { get; }

    public IntegrationTestServer(params MockService[] mockServices)
    {
        var factory = new IntegrationTestWebApplicationFactory<Program>(mockServices);
        HttpClient = factory.CreateClient();

        var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
        Channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
    }

    public void Dispose()
    {
        Channel.Dispose();
        GC.SuppressFinalize(this);
    }
}