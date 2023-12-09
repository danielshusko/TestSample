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

        var integrationTestServer = new IntegrationTestServer(new MockService(typeof(IUserService), _mockUserService.Object));
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

        var expectedResult = new UserMessage
                             {
                                 Id = 1,
                                 FirstName = firstName,
                                 LastName = lastName
                             };

        _mockUserService
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string _, string fName, string lName) => new Result<User>(new User(1, fName, lName)));

        // Act
        var result = _usersClient.Execute<UserMessage>(request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Data.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Call_WithUnexpectedException_ThrowsInternalException()
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
        _mockUserService
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new NullReferenceException());

        // Act
        var result = _usersClient.Execute<RestTranscodedFailure>(request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.Data!.Code.Should().Be((int) StatusCode.Internal);
        result.Data!.Message.Should().Be("Internal error.");
    }

    [Fact]
    public void Call_WithNotFoundException_ThrowsNotFoundException()
    {
        // Arrange
        var firstName = "first";
        var lastName = "last";
        var errorMessage = "not found";

        var userFirstAndLastNameMessage = new UserFirstAndLastNameMessage
                                          {
                                              FirstName = firstName,
                                              LastName = lastName
                                          };
        var request = new RestRequest("api/users", Method.Post);

        request.AddJsonBody(userFirstAndLastNameMessage);
        _mockUserService
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new TestSampleNotFoundException(errorMessage));

        // Act
        var result = _usersClient.Execute<RestTranscodedFailure>(request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Data!.Code.Should().Be((int) StatusCode.NotFound);
        result.Data!.Message.Should().Be(errorMessage);
    }

    [Fact]
    public void Call_WithValidationException_ThrowsInvalidArgumentException()
    {
        // Arrange
        var firstName = "first";
        var lastName = "last";
        var errorMessage = "invalid";

        var userFirstAndLastNameMessage = new UserFirstAndLastNameMessage
                                          {
                                              FirstName = firstName,
                                              LastName = lastName
                                          };
        var request = new RestRequest("api/users", Method.Post);

        request.AddJsonBody(userFirstAndLastNameMessage);
        _mockUserService
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new TestSampleValidationException(errorMessage));

        // Act
        var result = _usersClient.Execute<RestTranscodedFailure>(request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Data!.Code.Should().Be((int) StatusCode.InvalidArgument);
        result.Data!.Message.Should().Be(errorMessage);
    }
}