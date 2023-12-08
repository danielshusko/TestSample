using TestSample.Domain.Exceptions;

namespace TestSample.Domain.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public User Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            throw new TestSampleValidationException("First and Last name cannot be empty.");
        }
        return _userRepository.Create(firstName, lastName);
    }

    public User GetById(int userId)
    {
        var user = _userRepository.GetById(userId);
        return user ?? throw new TestSampleNotFoundException("User not found.");
    }
}
