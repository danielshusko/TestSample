using TechTalk.SpecFlow;
using TestSample.Domain.Exceptions;

namespace TestSample.Domain.Component.Tests.Support;

public static class ScenarioContextExtensions
{
    public static T? GetByKey<T>(this ScenarioContext scenarioContext, ScenarioContextKey key, T? defaultValue = default) =>
        scenarioContext.TryGetValue(key.ToString(), out var value)
            ? (T) value
            : defaultValue;

    public static void SetByKey<T>(this ScenarioContext scenarioContext, ScenarioContextKey key, T? value) =>
        scenarioContext[key.ToString()] = value;

    public static T Get<T>(this ScenarioContext scenarioContext, string key)
    {
        if (!scenarioContext.ContainsKey(key))
        {
            throw new TestSampleNotFoundException("Key not found");
        }

        return (T) scenarioContext[key];
    }
}