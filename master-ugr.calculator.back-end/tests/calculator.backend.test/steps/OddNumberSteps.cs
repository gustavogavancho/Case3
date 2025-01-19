using System.Text.Json;
using TechTalk.SpecFlow;

namespace calculator.frontend.tests.steps;

[Binding]
public class OddNumberSteps
{
    private readonly ScenarioContext _scenarioContext;

    public OddNumberSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"a number (.*)")]
    public void GivenANumber(int number)
    {
        _scenarioContext.Add("number", number);
    }

    [When(@"I check if it is odd")]
    public void WhenICheckIfItIsOdd()
    {
        using (var client = new HttpClient())
        {
            var number = _scenarioContext.Get<int>("number");
            var urlBase = _scenarioContext.Get<string>("urlBase");
            var url = $"{urlBase}/api/Calculator/";
            var api_call = $"{url}number_attribute?number={number}";
            var response = client.GetAsync(api_call).Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonDocument = JsonDocument.Parse(responseBody);
            var odd = jsonDocument.RootElement.GetProperty("odd").GetBoolean();
            var prime = jsonDocument.RootElement.GetProperty("prime").GetBoolean();
            _scenarioContext.Add("isOdd", odd);
        }
    }

    [Then(@"it should be odd (.*)")]
    public async Task ThenItShouldBeOddTrue(bool isIt_boolean)
    {
        var isOdd = _scenarioContext.Get<bool>("isOdd");
        Assert.Equal(isOdd, isIt_boolean);
    }
}