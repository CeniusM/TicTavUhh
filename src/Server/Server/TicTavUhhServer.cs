

using static TicTavUhhServer.ILogger;

namespace TicTavUhhServer
{
    public class Server
    {
        
        public bool Running { get; private set; }

        private ILogger logger;
        public (string Message, LogWarningLevel WarningLevel) GetNextLog() => logger.GetLog();

        public Server() : this(new Logger()) { }

        public Server(ILogger logger)
        {
            this.logger = logger;
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}