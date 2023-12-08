namespace TestSample.Domain.Users;

public interface IUserService
{
    Task<Result<User>> Create(string tenantId, string firstName, string lastName);
    Task<Result<User>> GetById(string tenantId, int userId);
}