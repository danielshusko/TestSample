using TechTalk.SpecFlow;

namespace TestSample.Tests.Framework.Specflow;

public static class ScenarioInitializer
{
    private const string DefaultCultureCode = "en-US";
    private const int TenantIdMaxLength = 25;

    public static void InitializeScenario(FeatureContext featureContext, ScenarioContext scenarioContext)
    {
        var featureName = featureContext.FeatureInfo.Title;

        var currentTenantId = scenarioContext.GetByKey<string>(ScenarioContextKey.TenantId);
        if (string.IsNullOrWhiteSpace(currentTenantId) || !currentTenantId.StartsWith(featureName + "_"))
        {
            var formattedTenantId = TruncateString($"{featureName}_{Guid.NewGuid()}", TenantIdMaxLength);
            scenarioContext.SetByKey(ScenarioContextKey.TenantId, formattedTenantId);
            scenarioContext.SetByKey(ScenarioContextKey.UserId, formattedTenantId);
            scenarioContext.SetByKey(ScenarioContextKey.CultureCode, DefaultCultureCode);
        }
    }

    private static string TruncateString(string value, int maxLength) =>
        value.Length <= maxLength
            ? value
            : value[..maxLength];
}