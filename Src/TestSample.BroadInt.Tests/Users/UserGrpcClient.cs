using Grpc.Core;
using TestSample.Domain;
using TestSample.Grpc.proto;
using TestSample.Tests.Framework.TestServer;

namespace TestSample.BroadInt.Tests.Users;
public class UserGrpcClient
{
    private readonly Grpc.proto.Users.UsersClient _client;
    public UserGrpcClient()
    {
        var testServer = new IntegrationTestServer();
        _client = new Grpc.proto.Users.UsersClient(testServer.Channel);
    }

    public Result<UserMessage, RpcException> Create(string tenantId, string firstName, string lastName)
    {
        try
        {
            return _client.Create(
                new CreateUserRequestMessage
                {
                    TenantId = tenantId,
                    FirstName = firstName,
                    LastName = lastName
                });
        }
        catch (RpcException ex)
        {
            return ex;
        }
    }

    public Result<UserMessage, RpcException> GetById(int id)
    {
        try
        {
            return _client.GetById(
                new IdMessage
                {
                    Id = id
                });
        }
        catch (RpcException ex)
        {
            return ex;
        }
    }
}
