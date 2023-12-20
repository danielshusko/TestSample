using System.Net;
using Grpc.Core;

namespace TestSample.Grpc.NarrowInt.Tests;

public class ApiResult<T>
{
    public T? Value { get; }
    public int StatusCode { get; }
    public Exception? Exception { get; }
    public string? ErrorMessage { get; }

    public ApiResult(int statusCode, T value)
    {
        StatusCode = statusCode;
        Value = value;
        Exception = null;
        ErrorMessage = null;
    }

    public ApiResult(int statusCode, string errorMessage, Exception? exception)
    {
        StatusCode = statusCode;
        Value = default;
        Exception = exception;
        ErrorMessage = errorMessage;
    }

    public ApiResult(StatusCode statusCode, string errorMessage, Exception? exception)
        : this((int) statusCode, errorMessage, exception) { }

    public ApiResult(HttpStatusCode statusCode, string errorMessage, Exception? exception)
        : this((int) statusCode, errorMessage, exception) { }

    public static ApiResult<T> GrpcSuccess(T value) => new((int) global::Grpc.Core.StatusCode.OK, value);
    public static ApiResult<T> HttpSuccess(HttpStatusCode statusCode, T value) => new((int) statusCode, value);

    public StatusCode GetGrpcStatusCode() => (StatusCode) StatusCode;
    public bool IsGrpcSuccess() => GetGrpcStatusCode() == global::Grpc.Core.StatusCode.OK;

    public HttpStatusCode GetHttpStatusCode() => (HttpStatusCode) StatusCode;
    public bool IsHttpSuccess() => StatusCode is >= 200 and < 300;
}