# ticketmaster-checker

### How to run the application

1. Install the .NET 8.0 SDK
2. Clone the repository
3. Build the application with `dotnet build`
4. Run the application providing two arguments `TicketmasterChecker.exe URL URL_DESCRIPTION`  
	1. 4.1. The first argument is URL:
		1. As a samples argument value: `"https://www.ticketmaster.com.br/event/venda-geral-linkin-park"`
	1. 4.2. The second argument is URL_DESCRIPTION:
		1. As a samples argument value: `"Linkin Park - 15/11"`  
5. In this case, the full command would be:
	1. `TicketmasterChecker.exe "https://www.ticketmaster.com.br/event/venda-geral-linkin-park" "Linkin Park - 15/11"`
6. The application will check the availability of the tickets every 10 seconds and will notify you when any tickets are available.
