

namespace TicTavUhhServer
{
    public class Server
    {
        private ILogger logger;
        public string GetNextLog => logger.GetLog();

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