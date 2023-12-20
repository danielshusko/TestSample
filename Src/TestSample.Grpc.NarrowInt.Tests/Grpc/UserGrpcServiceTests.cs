using FluentAssertions;
using TestSample.Tests.Framework.TestServer;
using Xunit;

namespace TestSample.Grpc.NarrowInt.Tests.Grpc;

public class UserGrpcServiceTests
{
    private readonly UserGrpcClient _client = new(new IntegrationTestServer());

    [Fact]
    public void Create_WithValidData_ReturnsUser()
    {
        // Arrange
        var firstName = "first";
        var lastName = "last";

        // Act
        var result = _client.Create(firstName, lastName);

        // Assert
        result.IsGrpcSuccess().Should().BeTrue();
        result.Value!.Id.Should().BeGreaterThan(0);
        result.Value.FirstName.Should().Be(firstName);
        result.Value.LastName.Should().Be(lastName);
    }
}