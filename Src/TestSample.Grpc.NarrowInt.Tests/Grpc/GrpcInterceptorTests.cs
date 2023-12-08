using FluentAssertions;
using Grpc.Core;
using Moq;
using TestSample.Domain;
using TestSample.Domain.Exceptions;
using TestSample.Domain.Users;
using TestSample.Grpc.NarrowInt.Tests.Mocks;
using TestSample.Grpc.proto;
using Xunit;

namespace TestSample.Grpc.NarrowInt.Tests.Grpc;

public class GrpcInterceptorTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Users.UsersClient _usersClient;

    public GrpcInterceptorTests()
    {
        _mockUserService = new Mock<IUserService>();

        var integrationTestServer = new IntegrationTestServer(new MockService(typeof(IUserService), _mockUserService.Object));
        _usersClient = new Users.UsersClient(integrationTestServer.Channel);
    }

    [Fact]
    public void Call_WithSuccess_ReturnsResult()
    {
        // Arrange
        var firstName = "first";
        var lastName = "last";

        _mockUserService
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string fName, string lName) => new Result<User>(new User(1, fName, lName)));

        // Act
        var result = _usersClient.Create(
            new CreateUserRequestMessage
            {
                FirstName = firstName,
                LastName = lastName
            });

        // Assert
        result.Id.Should().Be(1);
        result.FirstName.Should().Be(firstName);
        result.LastName.Should().Be(lastName);
    }

    [Fact]
    public void Call_WithUnexpectedException_ThrowsInternalException()
    {
        // Arrange
        var firstName = "first";
        var lastName = "last";

        _mockUserService
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new NullReferenceException());

        // Act
        var grpcCall = () => _usersClient.Create(
            new CreateUserRequestMessage
            {
                FirstName = firstName,
                LastName = lastName
            });

        // Assert
        grpcCall.Should().Throw<RpcException>()
                .Where(x => x.StatusCode == StatusCode.Internal && x.Status.Detail == "Internal error.");
    }

    [Fact]
    public void Call_WithNotFoundException_ThrowsNotFoundException()
    {
        // Arrange
        var firstName = "first";
        var lastName = "last";
        var errorMessage = "not found";

        _mockUserService
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new TestSampleNotFoundException(errorMessage));

        // Act
        var grpcCall = () => _usersClient.Create(
            new CreateUserRequestMessage
            {
                FirstName = firstName,
                LastName = lastName
            });

        // Assert
        grpcCall.Should().Throw<RpcException>()
                .Where(x => x.StatusCode == StatusCode.NotFound && x.Status.Detail == errorMessage);
    }

    [Fact]
    public void Call_WithValidationException_ThrowsInvalidArgumentException()
    {
        // Arrange
        var firstName = "first";
        var lastName = "last";
        var errorMessage = "invalid";

        _mockUserService
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new TestSampleValidationException(errorMessage));

        // Act
        var grpcCall = () => _usersClient.Create(
            new CreateUserRequestMessage
            {
                FirstName = firstName,
                LastName = lastName
            });

        // Assert
        grpcCall.Should().Throw<RpcException>()
                .Where(x => x.StatusCode == StatusCode.InvalidArgument && x.Status.Detail == errorMessage);
    }
}