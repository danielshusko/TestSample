using TechTalk.SpecFlow;
using TestSample.Domain.Users;

namespace TestSample.Domain.Component.Tests.Users;

[Binding]
public class UserWhenSteps : UserBaseSteps
{
    protected UserWhenSteps(ScenarioContext scenarioContext, UserService userService)
        : base(scenarioContext, userService) { }

    [When(@"a user is created with first name ""([^""]*)"" and last name ""([^""]*)""")]
    public void WhenAUserIsCreatedWithFirstNameAndLastName(string first, string last)
    {
        var user = UserService.Create(TenantId, first, last).Result;
        ScenarioContext.Set(user);
    }
}