using TicketmasterChecker;

Console.WriteLine($"""
            ========================================================
            Starting the Ticketmaster Checker at {DateTime.Now}
            ========================================================
            """
);

new TicketmasterCore("https://www.ticketmaster.com.br/event/venda-geral-linkin-park", "Linkin Park - 15/11")
    .DoWork()
    .Wait();

//new TicketmasterCore("https://www.ticketmaster.com.br/event/venda-geral-linkin-park-extra", "Linkin Park - 16/11")
//    .DoWork()
//    .Wait();

Console.WriteLine($"""
            ========================================================
            Ending the Ticketmaster Checker at {DateTime.Now}
            ========================================================
            """
);
