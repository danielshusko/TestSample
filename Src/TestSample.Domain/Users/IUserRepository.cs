namespace TestSample.Domain.Users;
public interface IUserRepository
{
    User Create(string firstName, string lastName);
    User? GetById(int id);
}
