using System.Net;
using System.Net.Sockets;

namespace TicTavUhhServer;

internal class TicTavUhhGame
{
    public enum GameState
    {
        Standby,
        StartingUp,
        Stopped,
        Running
    }

    internal TicTacToe game = new TicTacToe();

    internal GameState gameState = GameState.Standby;
    internal bool JustStopped = false;

    internal ClientConnection player1;
    internal ClientConnection player2;

    public TicTavUhhGame()
    {

    }

    public void Play(ClientConnection player1, ClientConnection player2)
    {
        gameState = GameState.Running;
        this.player1 = player1;
        this.player2 = player2;
    }

    public void Update()
    {
        if (gameState != GameState.Running)
            throw new Exception("Trying to start a game that is not running");

        SendBoard(player1);
        SendBoard(player2);

        if (player1.connectionLevel != ClientConnection.ConnectionLevel.Connected)
        {
            Stop();
        }
        if (player2.connectionLevel != ClientConnection.ConnectionLevel.Connected)
        {
            Stop();
        }
    }

    public void Stop()
    {
        gameState = GameState.Stopped;

        this.player1 = null;
        this.player2 = null;

        gameState = GameState.Standby;
    }

    public void SendBoard(ClientConnection reciver)
    {
        //byte[] gameBytes = new byte[
        //    // Board, ( 0 = nothing, 1 = x, 2 = y)
        //    sizeof(int) * 9 +

        //    // Player turn, ( 1 = x, 2 = y )
        //    sizeof(int) * 1 +

        //    // Winner, ( 0 = nothing, 3 = draw, 1 = player1, 2 = player2 )
        //    sizeof(int) * 1
        //    ];

        List<byte> gameBytes = new List<byte>();

        // Get the board in to the byte array
        for (int i = 0; i < 9; i++)
            gameBytes.Add((byte)game.board[i]);
        gameBytes.Add((byte)game.player);
        gameBytes.Add((byte)game.winner);

        reciver.SendData(gameBytes);
    }

    ~TicTavUhhGame()
    {

    }
}
