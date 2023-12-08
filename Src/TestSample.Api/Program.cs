using TestSample.Domain.DependencyInjection;
using TestSample.Grpc.DependencyInjection;
using TestSample.PostgreSql.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
       .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
       .AddDomain()
       .AddGrpcServices()
       .AddPostgreSql(builder.Configuration);

builder.WebHost.UseKestrel(
    options =>
    {
        // This was added to resolve an issue with HTTPS being terminated at the service mesh layer.
        // See https://github.com/dotnet/aspnetcore/issues/30532 and https://github.com/istio/istio/issues/41102 for more info
        options.AllowAlternateSchemes = true;
    });

var app = builder.Build();

app.MapGrpcServices();
app.MigrateContext(app.Services);
app.Run();

namespace TestSample.Api
{
    public abstract class Program { }
}