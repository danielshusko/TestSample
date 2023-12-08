using System.Threading.Tasks;
using Grpc.Core;
using TestSample.Domain.Users;
using TestSample.Grpc.proto;

namespace TestSample.Grpc;

public class UserGrpcService : Users.UsersBase
{
    private readonly IUserService _userService;

    public UserGrpcService(IUserService userService)
    {
        _userService = userService;
    }

    public override Task<UserMessage> Create(CreateUserRequestMessage request, ServerCallContext context)
    {
        var user = _userService.Create(request.FirstName, request.LastName);
        return Task.FromResult(
            new UserMessage
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
    }

    public override Task<UserMessage> GetById(IdMessage request, ServerCallContext context)
    {
        var user = _userService.GetById(request.Id);
        return Task.FromResult(
            new UserMessage
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
    }
}