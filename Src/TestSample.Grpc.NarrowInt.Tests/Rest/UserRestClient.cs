using RestSharp;
using TestSample.Domain.Users;
using TestSample.Grpc.proto;
using TestSample.Tests.Framework.TestServer;

namespace TestSample.Grpc.NarrowInt.Tests.Rest;

public class UserRestClient : BaseRestClient, IUserClient
{
    public UserRestClient(IntegrationTestServer testServer) : base(testServer) { }

    public ApiResult<User> Create(string firstName, string lastName)
    {
        var request = new RestRequest("/api/users", Method.Post);
        request.AddJsonBody(
            new UserFirstAndLastNameMessage
            {
                FirstName = firstName,
                LastName = lastName
            });

        return ExecuteRequest<UserMessage, User>(
            request,
            x => new User(x.Id, x.FirstName, x.LastName)
        );
    }
}