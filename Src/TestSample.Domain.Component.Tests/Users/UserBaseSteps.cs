using TechTalk.SpecFlow;
using TestSample.Domain.Users;

namespace TestSample.Domain.Component.Tests.Users;

public class UserBaseSteps
{
    protected readonly ScenarioContext ScenarioContext;
    protected readonly UserService UserService;

    protected UserBaseSteps(ScenarioContext scenarioContext, UserService userService)
    {
        ScenarioContext = scenarioContext;
        UserService = userService;
    }
}