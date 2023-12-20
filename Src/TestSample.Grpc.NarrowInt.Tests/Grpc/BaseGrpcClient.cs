using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using TestSample.Tests.Framework.TestServer;

namespace TestSample.Grpc.NarrowInt.Tests.Grpc;

public class BaseGrpcClient : BaseClient
{
    protected readonly GrpcChannel Channel;
    protected readonly CallInvoker ChannelWithHeaders;

    protected BaseGrpcClient(IntegrationTestServer testServer)
    {
        Channel = testServer.Channel;
        ChannelWithHeaders = Channel.Intercept(
            metadata =>
            {
                foreach (var header in GetRequestHeaders())
                {
                    metadata.Add(header.Key, header.Value);
                }

                return metadata;
            });
    }

    protected static ApiResult<TModel> ExecuteMethod<TResponse, TModel>(Func<TResponse> clientMethod, Func<TResponse, TModel> mapFunc)
    {
        try
        {
            var response = clientMethod();
            return ApiResult<TModel>.GrpcSuccess(mapFunc(response!));
        }
        catch (RpcException ex)
        {
            return new ApiResult<TModel>(
                ex.StatusCode,
                ex.Status.Detail,
                ex
            );
        }
        catch (Exception ex)
        {
            return new ApiResult<TModel>(
                StatusCode.Internal,
                ex.Message,
                ex
            );
        }
    }
}