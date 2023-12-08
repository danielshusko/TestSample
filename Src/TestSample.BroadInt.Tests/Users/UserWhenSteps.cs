using NUnit.Framework;
using TechTalk.SpecFlow;
using TestSample.Tests.Framework.Specflow;

namespace TestSample.BroadInt.Tests.Users;

[Binding]
public class UserWhenSteps : UserBaseSteps
{
    protected UserWhenSteps(ScenarioContext scenarioContext, UserGrpcClient client)
        : base(scenarioContext, client) { }

    [When(@"a user is created with first name ""([^""]*)"" and last name ""([^""]*)""")]
    public void WhenAUserIsCreatedWithFirstNameAndLastName(string first, string last)
    {
        var userResult = Client.Create(ScenarioContext.GetByKey<string>(ScenarioContextKey.TenantId)!, first, last);
        ScenarioContext.Set(userResult);
    }
}