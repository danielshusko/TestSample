using TechTalk.SpecFlow;
using TestSample.Tests.Framework.Specflow;

namespace TestSample.BroadInt.Tests.Users;

public class UserBaseSteps
{
    protected readonly UserGrpcClient Client;
    protected readonly ScenarioContext ScenarioContext;

    protected string TenantId => ScenarioContext.GetByKey<string>(ScenarioContextKey.TenantId)!;

    protected UserBaseSteps(ScenarioContext scenarioContext, UserGrpcClient client)
    {
        ScenarioContext = scenarioContext;
        Client = client;
    }
}