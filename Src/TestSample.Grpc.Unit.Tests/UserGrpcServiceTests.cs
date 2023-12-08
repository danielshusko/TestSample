using FluentAssertions;
using Grpc.Core;
using Moq;
using TestSample.Domain.Users;
using TestSample.Grpc.proto;
using Xunit;

namespace TestSample.Grpc.Unit.Tests
{
    public class UserGrpcServiceTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserGrpcService _userGrpcService;

        public UserGrpcServiceTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockUserService
                .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string firstName, string lastName) => new User(1, firstName, lastName));

            _userGrpcService = new UserGrpcService(_mockUserService.Object);
        }

        [Fact]
        public void Test()
        {
            // Arrange
            var firstName = "first";
            var lastName = "last";
            var request = new CreateUserRequestMessage
                          {
                              FirstName = firstName,
                              LastName = lastName
                          };

            // Act
            var result = _userGrpcService.Create(request, TestServerCallContext.Create()).Result;

            // Assert
            result.Id.Should().Be(1);
            result.FirstName.Should().Be(firstName);
            result.LastName.Should().Be(lastName);
        }
    }


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
}
