namespace TestSample.Domain.Users;

public interface IUserRepository
{
    Task<Result<User>> Create(string tenantId, string firstName, string lastName);
    Task<Result<User>> GetById(string tenantId, int id);
    Task<Result<User>> GetByFirstAndLastName(string tenantId, string firstName, string lastName);
}