using System.Net;
using FluentAssertions;
using Moq;
using RestSharp;
using TestSample.Domain;
using TestSample.Domain.Users;
using TestSample.Grpc.NarrowInt.Tests.Mocks;
using TestSample.Grpc.proto;
using Xunit;

namespace TestSample.Grpc.NarrowInt.Tests.Rest;

public class UserGrpcServiceTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly RestClient _usersClient;

    public UserGrpcServiceTests()
    {
        _mockUserService = new Mock<IUserService>();
        
        var integrationTestServer = new IntegrationTestServer(new MockService(typeof(IUserService), _mockUserService.Object));
        _usersClient = new RestClient(integrationTestServer.HttpClient);
    }

    [Fact]
    public void Create_WithValidData_ReturnsUser()
    {
        // Arrange
        var firstName = "first";
        var lastName = "last";

        var createUserRequestMessage = new CreateUserRequestMessage
                 {
                     FirstName = firstName,
                     LastName = lastName
                 };
        var request = new RestRequest("api/users", Method.Post);
        request.AddJsonBody(createUserRequestMessage);

        var expectedResult = new UserMessage
                             {
                                 Id = 1,
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
        result.Data.Should().BeEquivalentTo(expectedResult);
    }
}