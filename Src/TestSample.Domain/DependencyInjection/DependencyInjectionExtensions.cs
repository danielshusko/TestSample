using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using TestSample.Domain.Users;

namespace TestSample.Domain.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDomain(
        this IServiceCollection serviceCollection
    )
    {
        serviceCollection.AddScoped<IUserService, UserService>();

        return serviceCollection;
    }
}