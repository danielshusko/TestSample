using TestSample.Domain.Users;

namespace TestSample.Grpc.NarrowInt.Tests;

public interface IUserClient
{
    ApiResult<User> Create(string firstName, string lastName);
}