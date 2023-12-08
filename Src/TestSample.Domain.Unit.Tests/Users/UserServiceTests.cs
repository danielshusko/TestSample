using FluentAssertions;
using Moq;
using TestSample.Domain.Exceptions;
using TestSample.Domain.Users;
using Xunit;

namespace TestSample.Domain.Unit.Tests.Users;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUserRepository
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string firstName, string lastName) => new User(1, firstName, lastName));

        _userService = new UserService(_mockUserRepository.Object);
    }

    [Fact]
    public void Create_WithValidData_ReturnsNewUser()
    {
        // Arrange
        var firstName = "First";
        var lastName = "Name";

        // Act
        var result = _userService.Create(firstName, lastName).Result;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(1);
        result.Value!.FirstName.Should().Be(firstName);
        result.Value!.LastName.Should().Be(lastName);
    }

    [Fact]
    public void Create_WithEmptyFirstName_ThrowsException()
    {
        // Arrange
        var firstName = string.Empty;
        var lastName = "Name";

        // Act
        var result = _userService.Create(firstName, lastName).Result;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeOfType<TestSampleValidationException>();
    }
}