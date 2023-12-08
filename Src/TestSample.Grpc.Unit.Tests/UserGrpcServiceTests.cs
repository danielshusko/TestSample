using FluentAssertions;
using Moq;
using TestSample.Domain.Users;
using TestSample.Grpc.proto;
using Xunit;

namespace TestSample.Grpc.Unit.Tests;

public class UserGrpcServiceTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserGrpcService _userGrpcService;

    public UserGrpcServiceTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockUserService
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string firstName, string lastName) => new User(1, firstName, lastName));

        _userGrpcService = new UserGrpcService(_mockUserService.Object);
    }

    [Fact]
    public void Create_WithValidData_ReturnsUser()
    {
        // Arrange
        var firstName = "first";
        var lastName = "last";
        var request = new CreateUserRequestMessage
                      {
                          FirstName = firstName,
                          LastName = lastName
                      };

        // Act
        var result = _userGrpcService.Create(request, TestServerCallContext.Create()).Result;

        // Assert
        result.Id.Should().Be(1);
        result.FirstName.Should().Be(firstName);
        result.LastName.Should().Be(lastName);
    }
}