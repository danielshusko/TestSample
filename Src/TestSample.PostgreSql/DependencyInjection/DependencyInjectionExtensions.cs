using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TestSample.Domain.Users;
using TestSample.PostgreSql.Context;
using TestSample.PostgreSql.Options;
using TestSample.PostgreSql.Repositories;

namespace TestSample.PostgreSql.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers the Location PostgreSql objects with the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="configurationManager">The configuration manager.</param>
    public static IServiceCollection AddPostgreSql(
        this IServiceCollection serviceCollection,
        ConfigurationManager configurationManager
    )
    {
        var postgreSqlOptions = RegisterOptions<PostgreSqlOptions>(configurationManager, "PostgreSql", serviceCollection);

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        serviceCollection.AddDbContext<TestSampleContext>(
            options =>
            {
                /* Set no reset on close true here to avoid an issue where the table owner gets set to a specific DbaaS user
                 *  instead of the role when the connection resets, resulting in not having access to the tables we create:
                 * see https://stackoverflow.com/questions/59633027/how-to-set-default-owner-of-objects-created-by-ef-core-code-first-using-postgres
                 */
                var connectionString = $"USER ID = {postgreSqlOptions.UserId}; " +
                                       $"Password = {postgreSqlOptions.Password}; " +
                                       $"Server = {postgreSqlOptions.Server}; " +
                                       $"Port = {postgreSqlOptions.Port}; " +
                                       $"Database = {postgreSqlOptions.Database}; " +
                                       $"SearchPath = {postgreSqlOptions.Schema}; " +
                                       "Integrated Security = true; " +
                                       "No reset on close = true;" +
                                       "Pooling = true";

                options.UseNpgsql(
                    connectionString,
                    x =>
                    {
                        x.MigrationsHistoryTable("_TestSampleMigrationsHistory", postgreSqlOptions.Schema);
                    });
            });

        serviceCollection.AddScoped<IUserRepository, UserRepository>();

        return serviceCollection;
    }

    /// <summary>
    /// Runs the EF migrations.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public static WebApplication MigrateContext(this WebApplication app, IServiceProvider serviceProvider)
    {
        var postgresSqlOptions = serviceProvider.GetRequiredService<IOptions<PostgreSqlOptions>>();
        if (postgresSqlOptions.Value.RunMigrationOnStart)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<TestSampleContext>();
            context.Database.Migrate();
        }

        return app;
    }
    public static T RegisterOptions<T>(ConfigurationManager configurationManager, string sectionKey, IServiceCollection serviceCollection) where T : class, new()
    {
        var configSection = configurationManager.GetSection(sectionKey);
        serviceCollection.Configure<T>(configSection);

        var options = new T();
        configSection.Bind(options);

        return options;
    }
}