using Microsoft.EntityFrameworkCore;
using TestSample.PostgreSql.Models;
using TestSample.PostgreSql.Models.Mappings;

namespace TestSample.PostgreSql.Context;

public class TestSampleContext : DbContext
{
    public virtual DbSet<UserDataModel> Users => Set<UserDataModel>();

    public TestSampleContext(
        DbContextOptions options
    )
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        UserMap.ConfigureEntity(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
}