using TechTalk.SpecFlow;

namespace TestSample.BroadInt.Tests.Users;

public class UserBaseSteps
{
    protected readonly ScenarioContext ScenarioContext;
    protected readonly UserGrpcClient Client;

    protected UserBaseSteps(ScenarioContext scenarioContext, UserGrpcClient client)
    {
        ScenarioContext = scenarioContext;
        Client = client;
    }
}