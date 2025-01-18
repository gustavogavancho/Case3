using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace calculator.frontend.tests.hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;
        public Hooks(ScenarioContext scenarioContext) 
        {
            _scenarioContext = scenarioContext;
        }
        [BeforeScenario]
        public async Task PrepareScenario()
        {
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,
                Timeout = 60000
            });
            var context = await browser.NewContextAsync();
            var urlBase =
                Environment.GetEnvironmentVariable("CALCULATOR_FRONTEND_URL") ?? "https://localhost:7297";
            _scenarioContext.Add("urlBase", urlBase);
            var page = await context.NewPageAsync();
            _scenarioContext.Add("page",page);
        }
    }
}
