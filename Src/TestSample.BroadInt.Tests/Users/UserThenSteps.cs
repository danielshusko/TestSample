using FluentAssertions;
using Grpc.Core;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TestSample.Domain;
using TestSample.Domain.Users;
using TestSample.Grpc.proto;

namespace TestSample.BroadInt.Tests.Users;

[Binding]
public class UserThenSteps : UserBaseSteps
{
    protected UserThenSteps(ScenarioContext scenarioContext, UserGrpcClient client) : base(scenarioContext, client) { }

    [Then(@"the a user is returned with the following properties")]
    public void ThenTheAUserIsReturnedWithTheFollowingProperties(Table table)
    {
        var userResult = ScenarioContext.Get<Result<UserMessage, RpcException>>();
        userResult.IsSuccess.Should().BeTrue();

        var expectedUser = table.CreateInstance<User>();

        userResult.Value!.Should().BeEquivalentTo(
            expectedUser,
            options => options.Excluding(x => x.Id)
        );
    }

    [Then(@"an error with status code ""([^""]*)"" should be returned")]
    public void ThenAnErrorWithStatusCodeShouldBeReturned(StatusCode statusCode)
    {
        var userResult = ScenarioContext.Get<Result<UserMessage, RpcException>>();
        userResult.IsSuccess.Should().BeFalse();
        userResult.Error!.Status.StatusCode.Should().Be(statusCode);
    }

    [Then(@"an error with message ""([^""]*)"" should be returned")]
    public void ThenAnErrorWithMessageShouldBeReturned(string errorMessage)
    {
        var userResult = ScenarioContext.Get<Result<UserMessage, RpcException>>();
        userResult.IsSuccess.Should().BeFalse();
        userResult.Error!.Status.Detail.Should().Be(errorMessage);
    }
}