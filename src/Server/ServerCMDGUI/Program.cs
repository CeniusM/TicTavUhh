using TicTavUhhServer;

Server server = new Server();

server.Start();


while (true)
{
    var log = server.GetNextLog();
    Console.ForegroundColor = ConsoleColor.Green;
    if (log.Message != string.Empty)
        Console.WriteLine(log.Message);
    Thread.Sleep(1000);
}

server.Stop();

