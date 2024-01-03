using System.Net;
using RestSharp;
using TestSample.Tests.Framework.TestServer;

namespace TestSample.Grpc.NarrowInt.Tests.Rest;

/// <summary>
/// This is a base rest client
/// </summary>
public class BaseRestClient : BaseClient
{
    protected RestClient _client;

    public BaseRestClient(IntegrationTestServer testServer)
    {
        //Under the hood, we use RestSharp for our rest calls. We will define a client that uses the in-memory test server.
        _client = new RestClient(testServer.HttpClient);

        //We set up the base rest client to always add the default request headers.
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