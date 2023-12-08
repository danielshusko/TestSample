using FluentAssertions;
using Moq;
using TestSample.Domain;
using TestSample.Domain.Users;
using TestSample.Grpc.proto;
using TestSample.Tests.Framework.TestServer;
using Xunit;

namespace TestSample.Grpc.NarrowInt.Tests.Grpc;

public class UserGrpcServiceTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Users.UsersClient _usersClient;

    public UserGrpcServiceTests()
    {
        _mockUserService = new Mock<IUserService>();

        var integrationTestServer = new IntegrationTestServer(new MockService(typeof(IUserService), _mockUserService.Object));
        _usersClient = new Users.UsersClient(integrationTestServer.Channel);
    }

    [Fact]
    public void Create_WithValidData_ReturnsUser()
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
}