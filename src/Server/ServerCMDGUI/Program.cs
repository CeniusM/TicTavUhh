using System.Net;
using System.Net.Sockets;
using TicTavUhhServer;
using static TicTavUhhServer.ILogger;
using LogLevel = TicTavUhhServer.ILogger.LogWarningLevel;

Server server = new Server(new Logger(LogLevel.Empty));
var adress = IPAddress.Parse(Util.GetLocalIPAddress());
int port = 2453;

ConsoleColor GetColor(LogLevel log)
{
    if (log == LogLevel.Debug)
        return ConsoleColor.Gray;
    else if (log == LogLevel.Message)
        return ConsoleColor.White;
    else if (log == LogLevel.Info)
        return ConsoleColor.Blue;
    else if (log == LogLevel.Warning)
        return ConsoleColor.DarkYellow;
    else if (log == LogLevel.Critical)
        return ConsoleColor.Red;
    else if (log == LogLevel.Error)
        return ConsoleColor.DarkRed;
    return ConsoleColor.Green;
}

int PrintNextLog()
{
    var log = server.GetNextLog();

    Console.ForegroundColor = GetColor(log.WarningLevel);

    if (log.WarningLevel != LogLevel.Empty)
        Console.WriteLine(log.Message);
    else
        return -1;
    return 1;
}

void PrintAllLogs()
{
    while (PrintNextLog() == 1) { }
}

server.Start();
server.Start();

Task LogPrinter = new Task(() =>
{
    while (true)
    {
        PrintAllLogs();
        Thread.Sleep(100);
    }
});
LogPrinter.Start();
Thread.Sleep(2000);

TcpClient client1 = new TcpClient();
while (!client1.Connected)
{
    client1.Connect(adress, port);
}

TcpClient client2 = new TcpClient();
while (!client2.Connected)
{
    client2.Connect(adress, port);
}

Console.WriteLine("Contcted");

Thread.Sleep(100000);


server.Stop();

