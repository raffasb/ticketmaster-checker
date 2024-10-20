using OpenQA.Selenium;

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
    private const string AVAILABILITY_BUTTON_ELEMENT_ID = "show-button";
    private const string AVAILABILITY_DROPDOWN_ELEMENT_ID = "show-dropdown";

    public TicketmasterCore(IWebDriver webDriver, string url, string urlDescription)
    {
        ArgumentNullException.ThrowIfNull(webDriver, nameof(webDriver));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        ArgumentNullException.ThrowIfNull(urlDescription, nameof(urlDescription));

        _webDriver = webDriver;
        _url = url;
        _urlDescription = urlDescription;
    }

    public async Task DoWork()
    {
        while (true)
        {
            Console.WriteLine($"Checking tickets... {DateTime.Now} ({_urlDescription})");

            try
            {
                await _webDriver.Navigate().GoToUrlAsync(_url);

                _webDriver.FindElement(By.Id(AVAILABILITY_BUTTON_ELEMENT_ID))?.Click();

                var elementValue = _webDriver.FindElement(By.XPath($"//ul[@id='{AVAILABILITY_DROPDOWN_ELEMENT_ID}']/li/a"))?.Text;

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