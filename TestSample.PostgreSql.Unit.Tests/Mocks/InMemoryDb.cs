using Microsoft.EntityFrameworkCore;
using TestSample.PostgreSql.Context;

namespace TestSample.PostgreSql.Unit.Tests.Mocks;

internal static class InMemoryDb
{
    public static TestSampleContext NewTestSampleContext()
    {
        var context = new TestSampleContext(
            new DbContextOptionsBuilder<TestSampleContext>()
                .UseInMemoryDatabase("db").Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }
}