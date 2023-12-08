using System;
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
    /// <param name="useJsonTranscoding">When true, the .Net transcoding will be enabled.</param>
    /// <param name="interceptorsTypes">A collection of external interceptors that need to be mapped.</param>
    public static IServiceCollection AddGrpcServices(
        this IServiceCollection serviceCollection,
        bool useJsonTranscoding,
        params Type[] interceptorsTypes)
    {
        var grpcBuilder = serviceCollection
            .AddGrpc(
                options =>
                {
                    foreach (var interceptor in interceptorsTypes)
                    {
                        options.Interceptors.Add(interceptor);
                    }
                });

        if (useJsonTranscoding)
        {
            grpcBuilder.AddJsonTranscoding();
        }

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