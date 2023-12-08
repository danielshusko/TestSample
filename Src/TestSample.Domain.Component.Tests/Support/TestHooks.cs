using TechTalk.SpecFlow;
using TestSample.Tests.Framework.Specflow;

namespace TestSample.Domain.Component.Tests.Support;

[Binding]
public class TestHooks
{
    [BeforeScenario]
    public static void SetUpTenant(FeatureContext featureContext, ScenarioContext scenarioContext) =>
        ScenarioInitializer.InitializeScenario(featureContext, scenarioContext);
}