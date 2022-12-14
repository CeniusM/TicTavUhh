

using static TicTavUhhServer.ILogger;

namespace TicTavUhhServer
{
    public class Server
    {

        public bool Running { get; private set; }
        public bool StoppingServer { get; private set; }

        private ILogger logger;
        public (string Message, LogWarningLevel WarningLevel) GetNextLog() => logger.GetLog();

        public Server() : this(new Logger()) { }

        public Server(ILogger logger)
        {
            this.Running = false;
            this.StoppingServer = false;
            this.logger = logger;
        }

        /// <summary>
        /// Starts a new thread the server lives on, if there isent allready a thread running
        /// </summary>
        public void Start()
        {
            if (!Running)
            {
                Thread thread = new Thread(ServerLoop);
                thread.Start();
            }
        }

        private void ServerLoop()
        {
            Running = true;

            while (Running)
            {
                if (StoppingServer)
                    break;
            }

            Running = false;
        }

        public void Stop()
        {
            StoppingServer = true;
        }
    }
}