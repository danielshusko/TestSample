using TechTalk.SpecFlow;
using TestSample.Domain.Component.Tests.Support;
using TestSample.Domain.Users;
using TestSample.Tests.Framework.Specflow;

namespace TestSample.Domain.Component.Tests.Users;

[Binding]
public class UserWhenSteps : UserBaseSteps
{
    protected UserWhenSteps(ScenarioContext scenarioContext, UserService userService)
        : base(scenarioContext, userService) { }

    [When(@"a user is created with first name ""([^""]*)"" and last name ""([^""]*)""")]
    public void WhenAUserIsCreatedWithFirstNameAndLastName(string first, string last)
    {
        var user = UserService.Create(ScenarioContext.GetByKey<string>(ScenarioContextKey.TenantId)!, first, last).Result;
        ScenarioContext.Set(user);
    }
}