using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestSample.Domain;
using TestSample.Domain.Exceptions;
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

    public async Task<Result<User>> Create(string tenantId, string firstName, string lastName)
    {
        try
        {
            var dataModel = new UserDataModel
                            {
                                TenantId = tenantId,
                                FirstName = firstName,
                                LastName = lastName
                            };
            await _context.Users.AddAsync(dataModel);
            await _context.SaveChangesAsync();
            return new User(dataModel.Id, dataModel.FirstName, dataModel.LastName);
        }
        catch (Exception ex)
        {
            return new TestSampleException(ex.Message, TestSampleExceptionType.Unknown, ex);
        }
    }

    public async Task<Result<User>> GetById(string tenantId, int id)
    {
        var user = await _context.Users
                                 .Where(x => x.Id == id && x.TenantId == tenantId)
                                 .Select(x => new User(x.Id, x.FirstName, x.LastName))
                                 .FirstOrDefaultAsync();

        if (user == null)
        {
            return new TestSampleNotFoundException("User not found");
        }

        return user;
    }
}