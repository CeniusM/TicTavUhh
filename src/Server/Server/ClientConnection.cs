

using System.Net;
using System.Net.Sockets;

namespace TicTavUhhServer;

internal class ClientConnection
{
    internal enum ConnectionLevel
    {
        /// <summary>
        /// No connection and inactive
        /// </summary>
        Standby,

        /// <summary>
        /// Currently listening for a client
        /// </summary>
        Connecting,

        /// <summary>
        /// Paused is used for indicating if an allready established connection was lost,
        /// and can try to reconnect, if it fails it can go back to standby
        /// </summary>
        Paused,

        /// <summary>
        /// Indicates that a connecition is established with a client
        /// </summary>
        Connected
    }

    internal ConnectionLevel connectionLevel { get; private set; }

    internal IPAddress ThisIPAddress { get; set; }
    internal int hisPort { get; private set; }
    internal int ID { get; private set; }

    private TcpClient? client = null;

    public ClientConnection(IPAddress iPAddress, int port, int ID)
    {
        connectionLevel = ConnectionLevel.Standby;
        ThisIPAddress = iPAddress;
        hisPort = port;
        this.ID = ID;
    }

    public void EstablishConnection()
    {
        connectionLevel = ConnectionLevel.Connecting;
        TcpListener listener = new TcpListener(ThisIPAddress, hisPort);
        listener.Start();
        while (client == null || !client.Connected)
        {
            client = listener.AcceptTcpClientAsync().Result;
        }
        connectionLevel = ConnectionLevel.Connected;
    }

    public void SendData(List<byte> bytes) => SendData();
        
    public void SendData()
    {
        if (client.Connected)
        {

        }
        else
            connectionLevel = ConnectionLevel.Paused;
    }

    ~ClientConnection()
    {
        if (client is not null)
        {
            if (client.Connected)
                client.Close();
            client.Dispose();
        }
    }
}
