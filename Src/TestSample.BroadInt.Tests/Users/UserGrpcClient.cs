using Grpc.Core;
using TestSample.Domain;
using TestSample.Grpc.proto;

namespace TestSample.BroadInt.Tests.Users;

public class UserGrpcClient : BaseGrpcClient
{
    private readonly Grpc.proto.Users.UsersClient _client;

    public UserGrpcClient()
    {
        _client = new Grpc.proto.Users.UsersClient(Channel);
    }

    public Result<UserMessage, RpcException> Create(string tenantId, string firstName, string lastName)
    {
        try
        {
            return _client.Create(
                new UserFirstAndLastNameMessage
                {
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

    public Result<UserMessage, RpcException> GetByFirstAndLastName(string tenantId, string firstName, string lastName)
    {
        try
        {
            return _client.GetByFirstAndLastName(
                new UserFirstAndLastNameMessage
                {
                    FirstName = firstName,
                    LastName = lastName
                });
        }
        catch (RpcException ex)
        {
            return ex;
        }
    }
}