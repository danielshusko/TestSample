using Microsoft.AspNetCore.Http;

namespace TestSample.Domain;

public interface IRequestService
{
    string GetTenantId();
}
public class RequestService : IRequestService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetTenantId() => _httpContextAccessor.HttpContext.Request.Headers[Constants.TenantIdKey];
}

public static class Constants
{
    public const string TenantIdKey = "TenantId";
}