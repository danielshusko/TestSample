namespace TestSample.Domain.Users;

public interface IUserRepository
{
    Task<Result<User>> Create(string firstName, string lastName);
    Task<Result<User>> GetById(int id);
    Task<Result<User>> GetByFirstAndLastName(string firstName, string lastName);
}