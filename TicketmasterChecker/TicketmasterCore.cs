using OpenQA.Selenium;

namespace TicketmasterChecker;

internal sealed class TicketmasterCore
{
    private readonly IUserNotification _userNotification;
    private readonly IWebDriver _webDriver;
    private readonly string _url;
    private readonly string _urlDescription;

    private const int INTERATION_CHECK_INTERVAL_IN_MILLISECONDS = 10_000;
    private const string AVAILABILITY_KEYWORD_REFERENCE = "Esgotado";
    private const string AVAILABILITY_BUTTON_ELEMENT_ID = "show-button";
    private const string AVAILABILITY_DROPDOWN_ELEMENT_ID = "show-dropdown";

    public TicketmasterCore(IUserNotification userNotification, IWebDriver webDriver, string url, string urlDescription)
    {
        ArgumentNullException.ThrowIfNull(userNotification, nameof(userNotification));
        ArgumentNullException.ThrowIfNull(webDriver, nameof(webDriver));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        ArgumentNullException.ThrowIfNull(urlDescription, nameof(urlDescription));

        _userNotification = userNotification;
        _webDriver = webDriver;
        _url = url;
        _urlDescription = urlDescription;
    }

    public async Task DoWork(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine($"Checking tickets... {DateTime.Now} ({_urlDescription})");

            try
            {
                await _webDriver.Navigate().GoToUrlAsync(_url);

                _webDriver.FindElement(By.Id(AVAILABILITY_BUTTON_ELEMENT_ID))?.Click();

                var elementValue = _webDriver.FindElement(By.XPath($"//ul[@id='{AVAILABILITY_DROPDOWN_ELEMENT_ID}']/li/a"))?.Text;

                if (IsThereAnyTicketAvailable(elementValue))
                {
                    Console.WriteLine($"Tickets available at {DateTime.Now} ({_urlDescription})");

                    _userNotification.Notify();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred ({_urlDescription}): {ex.Message}");
            }

            await Task.Delay(INTERATION_CHECK_INTERVAL_IN_MILLISECONDS, cancellationToken);
        }
    }

    private bool IsThereAnyTicketAvailable(string? elementValue)
    {
        return !string.IsNullOrEmpty(elementValue) && !elementValue.Contains(AVAILABILITY_KEYWORD_REFERENCE, StringComparison.InvariantCultureIgnoreCase);
    }
}