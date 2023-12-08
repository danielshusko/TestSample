using Microsoft.EntityFrameworkCore;
using TestSample.PostgreSql.Models;

namespace TestSample.PostgreSql.Context;

public class TestSampleContext : DbContext
{
    public virtual DbSet<UserDataModel> Users => Set<UserDataModel>();

    public TestSampleContext(
        DbContextOptions options
    )
        : base(options) { }
}