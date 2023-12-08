using Microsoft.EntityFrameworkCore;
using TestSample.Grpc.NarrowInt.Tests.Mocks;
using TestSample.PostgreSql.Context;
using TestSample.PostgreSql.DependencyInjection;
using TestSample.PostgreSql.Options;

namespace TestSample.PostgreSql.NarrowInt.Tests;

public class BaseRepositoryTests
{
    protected TestSampleContext _context;

    protected BaseRepositoryTests()
    {
        _context = CreateContext();
    }

    private static TestSampleContext CreateContext()
    {
        var postgreSqlOptions = CreatePostgreSqlOptions();
        var connectionString = DependencyInjectionExtensions.CreateConnectionString(postgreSqlOptions);
        var optionsBuilder = new DbContextOptionsBuilder<TestSampleContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            x =>
            {
                x.MigrationsHistoryTable("_TestSampleMigrationsHistory", postgreSqlOptions.Schema);
            });
        return new TestSampleContext(optionsBuilder.Options);
    }

    protected static PostgreSqlOptions CreatePostgreSqlOptions() =>
        new()
        {
            Server = GetValueFromEnvironment(PostgresVariableKeys.Server, "localhost"),
            Database = GetValueFromEnvironment(PostgresVariableKeys.Database, "postgres"),
            Port = int.Parse(GetValueFromEnvironment(PostgresVariableKeys.Port, "5432")),
            UserId = GetValueFromEnvironment(PostgresVariableKeys.UserId, "postgres"),
            Password = GetValueFromEnvironment(PostgresVariableKeys.Password, "admin"),
            Schema = GetValueFromEnvironment(PostgresVariableKeys.Schema, "test_sample")
        };

    protected static string GetValueFromEnvironment(string key, string defaultValue)
    {
        var envValue = Environment.GetEnvironmentVariable(key);
        return string.IsNullOrWhiteSpace(envValue) ? defaultValue : envValue;
    }
}