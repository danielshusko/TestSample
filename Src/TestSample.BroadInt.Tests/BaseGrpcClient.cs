using Grpc.Net.Client;
using TestSample.Tests.Framework.TestServer;

namespace TestSample.BroadInt.Tests;

public class BaseGrpcClient
{
    protected GrpcChannel Channel { get; }

    protected BaseGrpcClient()
    {
        //TODO: make config driven
        var useLiveServer = false;
        if (useLiveServer)
        {
            Channel = GrpcChannel.ForAddress("http://localhost:9000");
        }
        else
        {
            var testServer = new IntegrationTestServer();
            Channel = testServer.Channel;
        }
    }
}