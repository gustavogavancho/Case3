using System.Text.Json;
using TechTalk.SpecFlow;

namespace calculator.lib.test.steps;

[Binding]
public class CalculatorSteps
{
    private readonly ScenarioContext _scenarioContext;

    public CalculatorSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"the first number is (.*)")]
    public void GivenTheFirstNumberIs(int firstNumber)
    {
        _scenarioContext.Add("firstNumber", firstNumber);
    }

    [Given(@"the second number is (.*)")]
    public void GivenTheSecondNumberIs(int secondNumber)
    {
        _scenarioContext.Add("secondNumber", secondNumber);
    }

    private void ApiCall(string operation)
    {
        using (var client = new HttpClient())
        {
            var urlBase = _scenarioContext.Get<string>("urlBase");
            var firstNumber = _scenarioContext.Get<int>("firstNumber");
            var secondNumber = _scenarioContext.Get<int>("secondNumber");
            var url = $"{urlBase}api/Calculator/";
            var api_call = $"{url}{operation}?a={firstNumber}&b={secondNumber}";
            var response = client.GetAsync(api_call).Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonDocument = JsonDocument.Parse(responseBody);

            if (jsonDocument.RootElement.GetProperty("result").ValueKind == JsonValueKind.String &&
                jsonDocument.RootElement.GetProperty("result").GetString() == "NaN")
            {
                _scenarioContext.Add("result", double.NaN);
            }
            else
            {
                var result = jsonDocument.RootElement.GetProperty("result").GetDouble();
                _scenarioContext.Add("result", result);
            }
        }
    }

    [When(@"the two numbers are added")]
    public void WhenTheTwoNumbersAreAdded()
    {
        ApiCall("add");
    }

    [When(@"I divide first number by second number")]
    public void WhenIDivideFirstNumberBySecondNumber()
    {
        ApiCall("divide");
    }

    [When(@"I multiply both numbers")]
    public void WhenIMultiplyBothNumbers()
    {
        ApiCall("multiply");
    }

    [When(@"I substract first number to second number")]
    public void WhenISubstractFirstNumberToSecondNumber()
    {
        ApiCall("subtract");
    }

    [Then(@"the result should be (.*)")]
    [Then(@"the result shall be (.*)")]
    [Then(@"the result is (.*)")]
    public void ThenTheResultShouldBe(string expectedResult)
    {
        var result = _scenarioContext.Get<double>("result");

        if (expectedResult == "NaN")
        {
            Assert.True(double.IsNaN(result), "Expected result to be NaN, but it was not.");
        }
        else
        {
            var numericExpectedResult = double.Parse(expectedResult);
            Assert.Equal(numericExpectedResult, result);
        }
    }
}