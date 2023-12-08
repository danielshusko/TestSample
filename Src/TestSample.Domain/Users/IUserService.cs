namespace TestSample.Domain.Users;

public interface IUserService
{
    User Create(string firstName, string lastName);
    User GetById(int userId);
}