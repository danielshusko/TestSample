using TestSample.Domain.Users;
using TestSample.Grpc.proto;
using TestSample.Tests.Framework.TestServer;

namespace TestSample.Grpc.NarrowInt.Tests.Grpc;

public class UserGrpcClient : BaseGrpcClient, IUserClient
{
    private readonly Users.UsersClient _client;

    public UserGrpcClient(IntegrationTestServer testServer)
        : base(testServer)
    {
        _client = new Users.UsersClient(ChannelWithHeaders);
    }

    public ApiResult<User> Create(string firstName, string lastName)
    {
        var request = new UserFirstAndLastNameMessage
        {
            FirstName = firstName,
            LastName = lastName
        };
        return ExecuteMethod(
            () => _client.Create(request),
            x => new User(x.Id, x.FirstName, x.LastName)
        );
    }
}