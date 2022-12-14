

using System.IO.Pipes;
using System.Net;
using static TicTavUhhServer.ILogger;

namespace TicTavUhhServer
{
    public class Server
    {
        private TicTavUhhGame[] Games = new TicTavUhhGame[1];

        public bool Running { get; private set; }
        public bool StoppingServer { get; private set; }

        private ILogger logger;
        public (string Message, LogWarningLevel WarningLevel) GetNextLog() => logger.GetLog();

        private const int PortUsed = 2453;
        private int IDCounter = 1;

        public Server() : this(new Logger(LogWarningLevel.Empty)) { }

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
                Running = true;
                logger.Log("Starting Server", LogWarningLevel.Info);
                Thread thread = new Thread(ServerLoop);
                thread.Start();
            }
            else
                logger.Log("Attempt at starting an allready running server", LogWarningLevel.Info);
        }

        private void ServerLoop()
        {
            Running = true;

            logger.Log("Initializing the games", LogWarningLevel.Info);
            for (int i = 0; i < Games.Length; i++)
            {
                Games[i] = new TicTavUhhGame();
            }

            while (Running)
            {
                if (StoppingServer)
                    break;

                for (int i = 0; i < Games.Length; i++)
                {
                    var game = Games[i];

                    if (game.JustStopped)
                    {
                        logger.Log("Game " + i + " just stopped", LogWarningLevel.Info);
                    }

                    if (game.gameState == TicTavUhhGame.GameState.Standby)
                    {
                        logger.Log("Starting Game: " + i, LogWarningLevel.Message);
                        game.gameState = TicTavUhhGame.GameState.StartingUp;
                        //Task task = new Task(() => FindPlayersForGame(Games[i], i));
                        //task.Start();
                        FindPlayersForGame(Games[i], i);
                    }
                    else if (game.gameState == TicTavUhhGame.GameState.Running)
                    {
                        game.Update();
                    }
                }

                Thread.Sleep(2000);
            }

            Running = false;
        }

        private async void FindPlayersForGame(TicTavUhhGame game, int index)
        {
            logger.Log("Finding players", LogWarningLevel.Debug);

            IPAddress adress = IPAddress.Parse(Util.GetLocalIPAddress());

            ClientConnection player1 = new ClientConnection(adress, PortUsed, GetUnikID());
            ClientConnection player2 = new ClientConnection(adress, PortUsed, GetUnikID());

            logger.Log("Finding player 1", LogWarningLevel.Debug);
            await player1.EstablishConnection();
            logger.Log("Game " + index + " Found player 1", LogWarningLevel.Debug);
            await player2.EstablishConnection();
            logger.Log("Game " + index + " Found player 2", LogWarningLevel.Debug);

            if (player1.connectionLevel == ClientConnection.ConnectionLevel.Connected)
                if (player2.connectionLevel == ClientConnection.ConnectionLevel.Connected)
                {
                    logger.Log("Game " + index + " Started", LogWarningLevel.Info);
                    game.Play(player1, player2);
                }
        }

        private int GetUnikID()
        {
            lock (this)
            {
                IDCounter++;
                return IDCounter;
            }
        }

        public void Stop()
        {
            logger.Log("Closing Server", LogWarningLevel.Message);
            StoppingServer = true;
        }
    }
}


/*
                logger.Log("Empty --", LogWarningLevel.Empty);
                logger.Log("Debug --", LogWarningLevel.Debug);
                logger.Log("Message --", LogWarningLevel.Message);
                logger.Log("Info --", LogWarningLevel.Info);
                logger.Log("Warning --", LogWarningLevel.Warning);
                logger.Log("Critical --", LogWarningLevel.Critical);
                logger.Log("Error --", LogWarningLevel.Error);
                Thread.Sleep(10000);
 */