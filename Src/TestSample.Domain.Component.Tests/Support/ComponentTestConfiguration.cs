using BoDi;
using Microsoft.EntityFrameworkCore;
using TestSample.Domain.Users;
using TestSample.PostgreSql.Context;
using TestSample.PostgreSql.Repositories;

namespace TestSample.Domain.Component.Tests.Support;

[Binding]
public class ComponentTestConfiguration
{
    private readonly IObjectContainer _objectContainer;

    public ComponentTestConfiguration(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeScenario]
    public void InitializeContainerConfiguration(FeatureContext featureContext, ScenarioContext scenarioContext)
    {
        ConfigureTenant(featureContext, scenarioContext);
        ConfigureContainer();
    }

    private void ConfigureContainer()
    {
        // Framework

        //Domain

        //Postgres
        var options = new DbContextOptionsBuilder<TestSampleContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
        var context = new TestSampleContext(
            options
        );
        _objectContainer.RegisterInstanceAs(context, typeof(TestSampleContext));
        _objectContainer.RegisterTypeAs<UserRepository, IUserRepository>();
    }

    private static void ConfigureTenant(
        FeatureContext featureContext,
        ScenarioContext scenarioContext) =>
        ScenarioInitializer.InitializeScenario(featureContext, scenarioContext);
}