using TechTalk.SpecFlow;
using TestSample.Domain.Users;
using TestSample.Tests.Framework.Specflow;

namespace TestSample.Domain.Component.Tests.Users;

public class UserBaseSteps
{
    protected readonly ScenarioContext ScenarioContext;
    protected readonly UserService UserService;
    protected string TenantId => ScenarioContext.GetByKey<string>(ScenarioContextKey.TenantId)!;

    protected UserBaseSteps(ScenarioContext scenarioContext, UserService userService)
    {
        ScenarioContext = scenarioContext;
        UserService = userService;
    }
}