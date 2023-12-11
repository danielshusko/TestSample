using FluentAssertions;
using Grpc.Core;
using TechTalk.SpecFlow;
using TestSample.Tests.Framework.Specflow;

namespace TestSample.BroadInt.Tests.Users;

[Binding]
public class UserGivenSteps : UserBaseSteps
{
    protected UserGivenSteps(ScenarioContext scenarioContext, UserGrpcClient client) : base(scenarioContext, client) { }

    [Given(@"a user exists with first name ""([^""]*)"" and last name ""([^""]*)""")]
    public void GivenAUserExistsWithFirstNameAndLastName(string first, string last)
    {
        var userResult = Client.GetByFirstAndLastName(TenantId, first, last);
        if (userResult is { IsSuccess: false, Error.StatusCode: StatusCode.NotFound })
        {
            userResult = Client.Create(ScenarioContext.GetByKey<string>(ScenarioContextKey.TenantId)!, first, last);
        }

        userResult.IsSuccess.Should().BeTrue();
    }
}