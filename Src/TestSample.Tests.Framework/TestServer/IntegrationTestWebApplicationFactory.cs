using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestSample.Tests.Framework.TestServer;

public class IntegrationTestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly MockService[] _mockServices;

    public IntegrationTestWebApplicationFactory(params MockService[] mockServices)
    {
        _mockServices = mockServices;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        SetPostgresEnvironmentVariables();

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(
            services =>
            {
                foreach (var mockService in _mockServices)
                {
                    services.AddScoped(mockService.ServiceType, _ => mockService.ServiceInstance);
                }
            });

        builder.UseEnvironment("Development");
    }

    protected void SetPostgresEnvironmentVariables()
    {
        SetEnvironmentVariableIfNull(PostgresVariableKeys.Server, "localhost");
        SetEnvironmentVariableIfNull(PostgresVariableKeys.Database, "postgres");
        SetEnvironmentVariableIfNull(PostgresVariableKeys.Port, "5432");
        SetEnvironmentVariableIfNull(PostgresVariableKeys.UserId, "postgres");
        SetEnvironmentVariableIfNull(PostgresVariableKeys.Password, "admin");
        SetEnvironmentVariableIfNull(PostgresVariableKeys.Schema, "test_sample");
    }

    protected void SetEnvironmentVariableIfNull(string key, string value)
    {
        var existingValue = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrWhiteSpace(existingValue))
        {
            Environment.SetEnvironmentVariable(key, value);
        }
    }
}