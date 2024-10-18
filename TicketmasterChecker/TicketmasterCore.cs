using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TicketmasterChecker;

internal sealed class TicketmasterCore
{
    private readonly string _url;
    private readonly string _urlDescription;
    private readonly IWebDriver _webDriver;

    private const int INTERATION_INTERVAL = 10_000;
    private const int BEEP_FREQUENCY = 1_000;
    private const int BEEP_DURATION = 5_000;

    private const string AVAILABILITY_KEYWORD_REFERENCE = "Esgotado";
    private const string AVAILABILITY_OPTIONS_ELEMENT_ID = "show-button";
    private const string AVAILABILITY_VALUES_ELEMENT_ID = "66923";

    public TicketmasterCore(string url, string urlDescription)
    {
        _url = url;
        _urlDescription = urlDescription;

        var options = new ChromeOptions();
        options.AddArgument("headless");

        _webDriver = new ChromeDriver(options);
    }

    public async Task DoWork()
    {
        while (true)
        {
            Console.WriteLine($"Checking tickets... {DateTime.Now} ({_urlDescription})");

            try
            {
                await _webDriver.Navigate().GoToUrlAsync(_url);

                _webDriver.FindElement(By.Id(AVAILABILITY_OPTIONS_ELEMENT_ID))?.Click();

                var elementValue = _webDriver.FindElement(By.Id(AVAILABILITY_VALUES_ELEMENT_ID))?.Text;

                if (IsThereAnyTicketAvailable(elementValue))
                {
                    NotifyUser();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred ({_urlDescription}): {ex.Message}");
            }

            await Task.Delay(INTERATION_INTERVAL);
        }
    }

    private bool IsThereAnyTicketAvailable(string? elementValue)
    {
        return !string.IsNullOrEmpty(elementValue) && !elementValue.Contains(AVAILABILITY_KEYWORD_REFERENCE, StringComparison.InvariantCultureIgnoreCase);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Known that is only a Windows capability")]
    private void NotifyUser()
    {
        Console.WriteLine($"Tickets available at {DateTime.Now} ({_urlDescription})");
        Console.Beep(BEEP_FREQUENCY, BEEP_DURATION);
    }
}