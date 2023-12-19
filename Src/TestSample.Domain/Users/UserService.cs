using TestSample.Domain.Exceptions;

namespace TestSample.Domain.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<Result<User>> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            return Task.FromResult(new Result<User>(new TestSampleValidationException("First and Last name cannot be empty.")));
        }

        return _userRepository.Create(firstName, lastName);
    }

    public Task<Result<User>> GetById(int userId) => _userRepository.GetById(userId);
    public Task<Result<User>> GetByFirstAndLastName(string firstName, string lastName) => _userRepository.GetByFirstAndLastName(firstName, lastName);
}