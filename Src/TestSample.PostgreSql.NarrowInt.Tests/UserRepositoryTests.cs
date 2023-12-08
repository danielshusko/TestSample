using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TestSample.Domain.Exceptions;
using TestSample.Domain.Users;
using TestSample.PostgreSql.Repositories;
using Xunit;

namespace TestSample.PostgreSql.NarrowInt.Tests;

public class UserRepositoryTests : BaseRepositoryTests
{
    private readonly string _tenantId;
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        _tenantId = Guid.NewGuid().ToString();
        _userRepository = new UserRepository(_context);
    }

    [Fact]
    public void Create_WithValidData_CreatesUser()
    {
        // Arrange
        var firstName = $"first_{DateTime.UtcNow:yyyyMMddHHmmssffff}";
        var lastName = $"last_{DateTime.UtcNow:yyyyMMddHHmmssffff}";

        var expectedResult = new User(-1, firstName, lastName);

        // Act
        var result = _userRepository.Create(_tenantId, firstName, lastName).Result;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().BeEquivalentTo(
            expectedResult,
            options => options.Excluding(x => x.Id)
        );
    }

    [Fact]
    public void Create_WithDuplicateUser_ReturnsException()
    {
        // Arrange
        var firstName = $"first_{DateTime.UtcNow:yyyyMMddHHmmssffff}";
        var lastName = $"last_{DateTime.UtcNow:yyyyMMddHHmmssffff}";

        // Act
        var result = _userRepository.Create(_tenantId, firstName, lastName).Result;
        result.IsSuccess.Should().BeTrue();
        result = _userRepository.Create(_tenantId, firstName, lastName).Result;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Type.Should().Be(TestSampleExceptionType.Unknown);
        result.Error!.InnerException.Should().BeOfType<DbUpdateException>();
        result.Error!.InnerException!.InnerException.Should().BeOfType<PostgresException>();
        result.Error!.InnerException!.InnerException.As<PostgresException>().Message.Should().StartWith("23505: duplicate key value violates unique constraint \"IX_User_TenantId_FirstName_LastName\"");
    }

    [Fact]
    public void Create_WithFirstNameTooLarge_ReturnsException()
    {
        // Arrange
        var firstName = new string('-', 500);
        var lastName = $"last_{DateTime.UtcNow:yyyyMMddHHmmssffff}";

        // Act
        var result = _userRepository.Create(_tenantId, firstName, lastName).Result;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error!.Type.Should().Be(TestSampleExceptionType.Unknown);
        result.Error!.InnerException.Should().BeOfType<DbUpdateException>();
        result.Error!.InnerException!.InnerException.Should().BeOfType<PostgresException>();
        result.Error!.InnerException!.InnerException.As<PostgresException>().Message.Should().Be("22001: value too long for type character varying(25)");
    }
}