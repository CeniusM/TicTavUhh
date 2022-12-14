

namespace TicTavUhhServer;

public interface ILogger
{
    /// <summary>
    /// Indicator of what type of Log is being pushed
    /// </summary>
    public enum LogWarningLevel
    {
        Empty,
        Debug,
        Message,
        Info,
        Warning,
        Critical,
        Error
    }

    /// <summary>
    /// Log a Message along with a WarningLevel
    /// </summary>
    /// <param name="Message"></param>
    /// <param name="WarningLevel"></param>
    internal void Log(string Message, LogWarningLevel WarningLevel);

    /// <summary>
    /// Returns the next message with its warning level. If there is no Logs left it returns Message empty string and -1 as LogWarningLevel
    /// </summary>
    /// <returns></returns>
    public (string Message, LogWarningLevel WarningLevel) GetLog();

    /// <summary>
    /// The level at wich and above the Logger accepts a log
    /// </summary>
    /// <param name="WarningLevel"></param>
    public void SetWarningLevel(LogWarningLevel WarningLevel);
}
