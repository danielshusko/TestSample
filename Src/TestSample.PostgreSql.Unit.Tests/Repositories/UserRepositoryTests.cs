using FluentAssertions;
using TestSample.PostgreSql.Repositories;
using TestSample.PostgreSql.Unit.Tests.Mocks;
using Xunit;

namespace TestSample.PostgreSql.Unit.Tests.Repositories;

public class UserRepositoryTests
{
    private const string TenantId = "tenant";

    private readonly UserRepository _userRepository = new(InMemoryDb.NewTestSampleContext());

    [Fact]
    public void Create_WithValidData_ReturnsUser()
    {
        // Arrange
        var firstName = "first";
        var lastName = "last";

        // Act
        var result = _userRepository.Create(TenantId, firstName, lastName).Result;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(1);
        result.Value!.FirstName.Should().Be(firstName);
        result.Value!.LastName.Should().Be(lastName);
    }
}