using TechTalk.SpecFlow;

namespace calculator.lib.test.steps;

[Binding]
public class OddNumberSteps
{
    private readonly ScenarioContext _scenarioContext;
    public OddNumberSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"a number (.*)")]
    public void WhenNumberIsChecked(int number)
    {
        _scenarioContext.Add("number", number);
    }

    [When("I check if it is odd")]
    public void ICheckIfItIsOdd()
    {
        var number = _scenarioContext.Get<int>("number");
        var isOdd = NumberAttributter.IsOdd(number);
        _scenarioContext.Add("isOdd", isOdd);
    }

    [Then("it should be odd (.*)")]
    public void ItShouldBeOdd(bool expected)
    {
        var isOdd = _scenarioContext.Get<bool>("isOdd");
        Assert.Equal(expected, isOdd);
    }
}