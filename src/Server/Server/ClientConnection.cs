

using System.Net;
using System.Net.Sockets;

namespace TicTavUhhServer;

internal class ClientConnection
{
    internal enum ConnectionLevel
    {
        Standby,
        Connecting,
        Paused,
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
