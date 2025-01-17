using TechTalk.SpecFlow;

namespace calculator.lib.test.steps;

[Binding]
public class PrimeNumberSteps
{
    private readonly ScenarioContext _scenarioContext;
    public PrimeNumberSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [When(@"number (.*) is checked")]
    public void WhenNumberIsChecked(int number)
    {
        var isPrime = NumberAttributter.IsPrime(number);
        _scenarioContext.Add("isPrime", isPrime);
    }

    [Then(@"the answer to know whether is prime or not is No")]
    public void ThenTheAnswerToKnowWhetherIsPrimeOrNotIsNo()
    {
        var isPrime = _scenarioContext.Get<bool>("isPrime");
        Assert.False(isPrime);
    }

    [Then(@"the answer to know whether is prime or not is Yes")]
    public void ThenTheAnswerToKnowWhetherIsPrimeOrNotIsYes()
    {
        var isPrime = _scenarioContext.Get<bool>("isPrime");
        Assert.True(isPrime);
    }
}