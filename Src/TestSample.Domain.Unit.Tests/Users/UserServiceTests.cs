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
        _userService = new UserService(_mockUserRepository.Object);
    }

    [Fact]
    public void Create_WithValidData_ReturnsNewUser()
    {
        // Arrange
        var firstName = "First";
        var lastName = "Name";

        _mockUserRepository
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string firstName, string lastName) => new User(1, firstName, lastName));

        // Act
        var result = _userService.Create(firstName, lastName);

        // Assert
        result.Id.Should().Be(1);
        result.FirstName.Should().Be(firstName);
        result.LastName.Should().Be(lastName);
    }

    [Fact]
    public void Create_WithEmptyFirstName_ThrowsException()
    {
        // Arrange
        var firstName = string.Empty;
        var lastName = "Name";

        _mockUserRepository
            .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string firstName, string lastName) => new User(1, firstName, lastName));

        // Act
        var testMethod =()=> _userService.Create(firstName, lastName);

        // Assert
        testMethod.Should().Throw<TestSampleValidationException>();
    }
}
