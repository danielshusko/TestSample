using System.Net;
using RestSharp;
using TestSample.Tests.Framework.TestServer;

namespace TestSample.Grpc.NarrowInt.Tests.Rest;

public class BaseRestClient : BaseClient
{
    protected RestClient _client;

    public BaseRestClient(IntegrationTestServer testServer)
    {
        _client = new RestClient(testServer.HttpClient);
        _client.AddDefaultHeaders(GetRequestHeaders());
    }

    public ApiResult<TModel> ExecuteRequest<TModel>(RestRequest request) => ExecuteRequest<TModel, TModel>(request, x => x);

    public ApiResult<TModel> ExecuteRequest<TResponse, TModel>(RestRequest request, Func<TResponse, TModel> mapFunc)
    {
        try
        {
            var response = _client.Execute<TResponse>(request);
            return response.IsSuccessful
                       ? ApiResult<TModel>.HttpSuccess(response.StatusCode, mapFunc(response.Data!))
                       : new ApiResult<TModel>(response.StatusCode, response.ErrorMessage ?? string.Empty, null);
        }
        catch (Exception ex)
        {
            return new ApiResult<TModel>(HttpStatusCode.InternalServerError, ex.Message, ex);
        }
    }
}