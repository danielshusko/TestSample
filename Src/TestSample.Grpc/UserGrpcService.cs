using System;
using System.Threading.Tasks;
using Grpc.Core;
using TestSample.Domain;
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

    public override async Task<UserMessage> Create(UserFirstAndLastNameMessage request, ServerCallContext context)
    {
        var userResult = await _userService.Create(request.TenantId, request.FirstName, request.LastName);
        return HandleResult(
            userResult,
            user => new UserMessage
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    }
        );
    }

    public override async Task<UserMessage> GetById(IdMessage request, ServerCallContext context)
    {
        var userResult = await _userService.GetById(request.TenantId, request.Id);
        return HandleResult(
            userResult,
            user => new UserMessage
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    }
        );
    }

    public override async Task<UserMessage> GetByFirstAndLastName(UserFirstAndLastNameMessage request, ServerCallContext context)
    {
        var userResult = await _userService.GetByFirstAndLastName(request.TenantId, request.FirstName, request.LastName);
        return HandleResult(
            userResult,
            user => new UserMessage
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    }
        );
    }

    private static TOut HandleResult<TResult, TOut>(Result<TResult> result, Func<TResult, TOut> mapFunc)
    {
        if (result.IsSuccess)
        {
            return mapFunc(result.Value!);
        }

        throw result.Error!;
    }
}