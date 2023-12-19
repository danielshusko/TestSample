using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using TestSample.Domain.Exceptions;

namespace TestSample.Grpc;

public class GrpcInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var response = await continuation(request, context);
            return response;
        }
        catch (Exception ex)
        {
            if (ex is TestSampleException testSampleException)
            {
                throw testSampleException.Type switch
                {
                    TestSampleExceptionType.NotFound => new RpcException(new Status(StatusCode.NotFound, ex.Message)),
                    TestSampleExceptionType.Validation => new RpcException(new Status(StatusCode.InvalidArgument, ex.Message)),
                    _ => new RpcException(new Status(StatusCode.Internal, "Internal error."))
                };
            }

            throw new RpcException(new Status(StatusCode.Internal, "Internal error."));
        }
    }
}