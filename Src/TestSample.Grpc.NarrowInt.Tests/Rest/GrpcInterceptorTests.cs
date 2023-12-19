using System.Net;
using FluentAssertions;
using Grpc.Core;
using Moq;
using RestSharp;
using TestSample.Domain;
using TestSample.Domain.Exceptions;
using TestSample.Domain.Users;
using TestSample.Grpc.proto;
using TestSample.Tests.Framework.TestServer;
using Xunit;

namespace TestSample.Grpc.NarrowInt.Tests.Rest;

public class GrpcInterceptorTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly RestClient _usersClient;

    public GrpcInterceptorTests()
    {
        _mockUserService = new Mock<IUserService>();

        var integrationTestServer = new IntegrationTestServer();
        _usersClient = new RestClient(integrationTestServer.HttpClient);
    }

    [Fact]
    public void Call_WithSuccess_ReturnsResult()
    {
        // Arrange
        var firstName = "first";
        var lastName = "last";

        var userFirstAndLastNameMessage = new UserFirstAndLastNameMessage
                                          {
                                              FirstName = firstName,
                                              LastName = lastName
                                          };
        var request = new RestRequest("api/users", Method.Post);
        request.AddJsonBody(userFirstAndLastNameMessage);
        request.AddHeader(Constants.TenantIdKey, Guid.NewGuid());

        var expectedResult = new UserMessage
                             {
                                 FirstName = firstName,
                                 LastName = lastName
                             };

        _mockUserService
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string fName, string lName) => new Result<User>(new User(1, fName, lName)));

        // Act
        var result = _usersClient.Execute<UserMessage>(request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data!.FirstName.Should().Be(firstName);
        result.Data!.LastName.Should().Be(lastName);
    }
}