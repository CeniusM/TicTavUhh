using LogLevel = TicTavUhhServer.ILogger.LogWarningLevel;

namespace TicTavUhhServer;

public class Logger : ILogger
{
    private Queue<(string Message, LogLevel WarningLevel)> logs;
    private LogLevel LogLevelFilter;

    public Logger(LogLevel LowestAcceptebleWarningLevel = LogLevel.Info)
    {
        logs = new Queue<(string Message, LogLevel WarningLevel)>();
        LogLevelFilter = LowestAcceptebleWarningLevel;
    }

    void ILogger.SetWarningLevel(LogLevel WarningLevel)
    {
        LogLevelFilter = WarningLevel;
    }

    (string Message, LogLevel WarningLevel) ILogger.GetLog()
    {
        if (logs.Count > 0)
            return logs.Dequeue();
        else
            return ("", LogLevel.Empty);
    }

    void ILogger.Log(string Message, LogLevel LogLevel)
    {
        if (LogLevel >= LogLevelFilter)
            logs.Enqueue((Message, LogLevel));
    }
}
