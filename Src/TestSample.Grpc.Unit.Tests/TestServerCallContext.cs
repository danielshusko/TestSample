using Grpc.Core;

namespace TestSample.Grpc.Unit.Tests;

public class TestServerCallContext : ServerCallContext
{
    private readonly Dictionary<object, object> _userState;

    public Metadata? ResponseHeaders { get; private set; }

    protected override string MethodCore => MethodName;
    protected override string HostCore => "HostName";
    protected override string PeerCore => "PeerName";
    protected override DateTime DeadlineCore { get; } = DateTime.UtcNow;
    protected override Metadata RequestHeadersCore { get; }

    protected override CancellationToken CancellationTokenCore { get; }

    protected override Metadata ResponseTrailersCore { get; }

    protected override Status StatusCore { get; set; }
    protected override WriteOptions? WriteOptionsCore { get; set; }
    protected override AuthContext AuthContextCore { get; }

    protected override IDictionary<object, object> UserStateCore => _userState;

    public string MethodName
    {
        get;
        set;
    } = "MethodName";

    private TestServerCallContext(Metadata requestHeaders, CancellationToken cancellationToken)
    {
        RequestHeadersCore = requestHeaders;
        CancellationTokenCore = cancellationToken;
        ResponseTrailersCore = new Metadata();
        AuthContextCore = new AuthContext(string.Empty, new Dictionary<string, List<AuthProperty>>());
        _userState = new Dictionary<object, object>();
    }

    protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options) =>
        throw new NotImplementedException();

    protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
    {
        if (ResponseHeaders != null)
        {
            throw new InvalidOperationException("Response headers have already been written.");
        }

        ResponseHeaders = responseHeaders;
        return Task.CompletedTask;
    }

    public static TestServerCallContext Create(Metadata? requestHeaders = null, CancellationToken cancellationToken = default) =>
        new(requestHeaders ?? new Metadata(), cancellationToken);
}