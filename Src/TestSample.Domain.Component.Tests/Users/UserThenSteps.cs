using TechTalk.SpecFlow.Assist;
using TestSample.Domain.Exceptions;
using TestSample.Domain.Users;

namespace TestSample.Domain.Component.Tests.Users;

[Binding]
public class UserThenSteps : UserBaseSteps
{
    protected UserThenSteps(ScenarioContext scenarioContext, UserService userService) : base(scenarioContext, userService) { }

    [Then(@"the user should be created with the following properties")]
    public void ThenTheUserShouldBeCreatedWithTheFollowingProperties(Table table)
    {
        var userResult = ScenarioContext.Get<Result<User>>();
        userResult.IsSuccess.Should().BeTrue();

        var expectedUser = table.CreateInstance<User>();
        userResult.Value!.Should().BeEquivalentTo(expectedUser);
    }

    [Then(@"an error with type ""([^""]*)"" should be returned")]
    public void ThenAnErrorWithTypeShouldBeReturned(TestSampleExceptionType exceptionType)
    {
        var userResult = ScenarioContext.Get<Result<User>>();
        userResult.IsSuccess.Should().BeFalse();
        userResult.Error!.Type.Should().Be(exceptionType);
    }
}