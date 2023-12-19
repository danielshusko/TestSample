using FluentAssertions;
using Grpc.Core;
using Moq;
using TestSample.Domain;
using TestSample.Domain.Exceptions;
using TestSample.Domain.Users;
using TestSample.Grpc.proto;
using TestSample.Tests.Framework.TestServer;
using Xunit;

namespace TestSample.Grpc.NarrowInt.Tests.Grpc;

public class GrpcInterceptorTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Users.UsersClient _usersClient;
    private readonly string _tenantId;

    public GrpcInterceptorTests()
    {
        _tenantId = Guid.NewGuid().ToString();
        _mockUserService = new Mock<IUserService>();

        var integrationTestServer = new IntegrationTestServer();
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
            new UserFirstAndLastNameMessage
            {
                FirstName = firstName,
                LastName = lastName
            },
            GetMetadata());

        // Assert
        result.Id.Should().BeGreaterThan(0);
        result.FirstName.Should().Be(firstName);
        result.LastName.Should().Be(lastName);
    }

    private Metadata GetMetadata() =>
        new()
        {
            { Constants.TenantIdKey, _tenantId }
        };
}