namespace TestSample.Domain.Users;

public interface IUserService
{
    Task<Result<User>> Create( string firstName, string lastName);
    Task<Result<User>> GetById(int userId);
    Task<Result<User>> GetByFirstAndLastName(string firstName, string lastName);
}