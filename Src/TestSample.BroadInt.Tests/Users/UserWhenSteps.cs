using TechTalk.SpecFlow;

namespace TestSample.BroadInt.Tests.Users;

[Binding]
public class UserWhenSteps : UserBaseSteps
{
    protected UserWhenSteps(ScenarioContext scenarioContext, UserGrpcClient client)
        : base(scenarioContext, client) { }

    [When(@"a user is created with first name ""([^""]*)"" and last name ""([^""]*)""")]
    public void WhenAUserIsCreatedWithFirstNameAndLastName(string first, string last)
    {
        var userResult = Client.Create(TenantId, first, last);
        ScenarioContext.Set(userResult);
    }

    [When(@"a user is retrieved with first name ""([^""]*)"" and last name ""([^""]*)""")]
    public void WhenAUserIsRetrievedWithFirstNameAndLastName(string first, string last)
    {
        var userResult = Client.GetByFirstAndLastName(TenantId, first, last);
        ScenarioContext.Set(userResult);
    }
}