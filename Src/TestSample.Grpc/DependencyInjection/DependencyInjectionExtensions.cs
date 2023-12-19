using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TestSample.Grpc.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
    /// <summary>
    ///     Registers the Location grpc objects in the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    public static IServiceCollection AddGrpcServices(
        this IServiceCollection serviceCollection)
    {
        var grpcBuilder = serviceCollection
            .AddGrpc(
                options =>
                {
                    options.Interceptors.Add(typeof(GrpcInterceptor));
                });

        grpcBuilder.AddJsonTranscoding();

        serviceCollection.AddGrpcReflection();
        serviceCollection.AddGrpcHealthChecks();

        return serviceCollection;
    }

    /// <summary>
    ///     Maps the Grpc services.
    /// </summary>
    /// <param name="webApplication">The web application.</param>
    public static WebApplication MapGrpcServices(this WebApplication webApplication)
    {
        webApplication.MapGrpcService<UserGrpcService>();
        webApplication.MapGrpcReflectionService();
        webApplication.MapGrpcHealthChecksService();

        return webApplication;
    }
}