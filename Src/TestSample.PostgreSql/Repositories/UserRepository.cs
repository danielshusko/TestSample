using System.Linq;
using TestSample.Domain.Users;
using TestSample.PostgreSql.Context;
using TestSample.PostgreSql.Models;

namespace TestSample.PostgreSql.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TestSampleContext _context;

    public UserRepository(TestSampleContext context)
    {
        _context = context;
    }

    public User Create(string firstName, string lastName)
    {
        var dataModel = new UserDataModel
                        {
                            FirstName = firstName,
                            LastName = lastName
                        };
        _context.Users.Add(dataModel);
        _context.SaveChanges();
        return new User(dataModel.Id, dataModel.FirstName, dataModel.LastName);
    }

    public User? GetById(int id) => _context.Users
                                            .Where(x => x.Id == id)
                                            .Select(x => new User(x.Id, x.FirstName, x.LastName))
                                            .FirstOrDefault();
}