using System.Net;
using FluentAssertions;
using TestSample.Grpc.proto;
using TestSample.Tests.Framework.TestServer;
using Xunit;

namespace TestSample.Grpc.NarrowInt.Tests.Rest;

public class UserGrpcServiceTests
{
    private readonly UserRestClient _client = new(new IntegrationTestServer());

    [Fact]
    public void Create_WithValidData_ReturnsUser()
    {
        // Arrange
        var firstName = "first";
        var lastName = "last";
        
        // Act
        var result = _client.Create(firstName, lastName);

        // Assert
        result.IsHttpSuccess().Should().BeTrue();
        result.GetHttpStatusCode().Should().Be(HttpStatusCode.OK);
        result.Value!.Id.Should().BeGreaterThan(0);
        result.Value.FirstName.Should().Be(firstName);
        result.Value.LastName.Should().Be(lastName);
    }
}