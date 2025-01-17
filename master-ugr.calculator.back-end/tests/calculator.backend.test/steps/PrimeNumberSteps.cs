using System.Text.Json;
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
        using (var client = new HttpClient())
        {
            var urlBase = _scenarioContext.Get<string>("urlBase");
            var url = $"{urlBase}api/Calculator/";
            var api_call = $"{url}is_prime?number={number}";
            var response = client.GetAsync(api_call).Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonDocument = JsonDocument.Parse(responseBody);
            var result = jsonDocument.RootElement.GetProperty("result").GetBoolean();
            _scenarioContext.Add("isPrime", result);
        }
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