using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TicketmasterChecker;

(string url, string urlDescription) = CheckArguments(args);

IWebDriver webDriver = CreateWebDriver();

Console.WriteLine($"""
            ========================================================
            Starting the Ticketmaster Checker at {DateTime.Now}
            ========================================================
            """
);

new TicketmasterCore(webDriver, url, urlDescription)
    .DoWork()
    .Wait();

Console.WriteLine($"""
            ========================================================
            Ending the Ticketmaster Checker at {DateTime.Now}
            ========================================================
            """
);

static (string url, string urlDescription) CheckArguments(string[] args)
{
    if (args.Length != 2)
    {
        throw new ArgumentException("You must provide the URL and the URL description as arguments");
    }

    string url = args[0];
    string urlDescription = args[1];

    return (url, urlDescription);
}

static IWebDriver CreateWebDriver()
{
    var options = new ChromeOptions();
    options.AddArgument("headless");

    return new ChromeDriver(options);
}