

using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

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
    internal int ThisPort { get; private set; }
    internal int ID { get; private set; }


    private TcpClient? client = null;
    private NetworkStream? stream = null;

    //private int bufferSize;
    private int bufferReadSize;

    private List<byte> bufferlist = new List<byte>();

    public ClientConnection(IPAddress iPAddress, int port, int ID, int bufferReadSize = 1)
    {
        connectionLevel = ConnectionLevel.Standby;
        ThisIPAddress = iPAddress;
        ThisPort = port;
        this.ID = ID;

        if (bufferReadSize < 1)
            bufferReadSize = 1;
        this.bufferReadSize = bufferReadSize;
    }

    public async Task EstablishConnection()
    {
        connectionLevel = ConnectionLevel.Connecting;
        TcpListener listener = new TcpListener(ThisIPAddress, ThisPort);
        //TcpListener listener = new TcpListener();
        listener.Start();
        while (client == null || !client.Connected)
        {   
            client = await listener.AcceptTcpClientAsync();
        }
        stream = client.GetStream();
        connectionLevel = ConnectionLevel.Connected;

        Task DataReciverTask = new Task(DataReciver);
        DataReciverTask.Start();
        listener.Stop();
    }

    public void EstablishConnection(TcpClient client)
    {
        if (!client.Connected)
            return;
        connectionLevel = ConnectionLevel.Connecting;
        this.client = client;
        stream = client.GetStream();
        connectionLevel = ConnectionLevel.Connected;

        Task DataReciverTask = new Task(DataReciver);
        DataReciverTask.Start();
    }

    public void Disconect()
    {
        if (!client.Connected)
            return;
        connectionLevel = ConnectionLevel.Paused;

        client.Close();

        connectionLevel = ConnectionLevel.Standby;
    }

    private async void DataReciver()
    {
Restart:
        byte[] buffer = new byte[bufferReadSize];
        byte[] ping = new byte[1] { 111 };
        try
        {
            while (client.Connected)
            {
                int dataReadSize = await stream.ReadAsync(buffer, 0, bufferReadSize);
                if (buffer[0] == 111)
                    stream.Write(ping);
                else
                    this.bufferlist.AddRange(buffer.Take(dataReadSize));
            }
        }
        catch (Exception e)
        {

        }
        connectionLevel = ConnectionLevel.Standby;
    }

    public int GetDataRecivedCount() => bufferlist.Count();

    public byte[] GetAllData()
    {
        byte[] buffer = this.bufferlist.
            ToArray();
        //Reverse().
        //ToArray();

        this.bufferlist.RemoveRange(0, buffer.Length);
        return buffer;
    }

    public void SendData(List<byte> bytes)
    {
        if (connectionLevel == ConnectionLevel.Connected)
            SendData(bytes.ToArray());
    }

    public void SendData(byte[] data)
    {
        if (connectionLevel == ConnectionLevel.Connected)
        {
            stream.Write(data);
        }
    }

    ~ClientConnection()
    {
        if (client is not null)
        {
            if (client.Connected)
                client.Close();
            client.Dispose();
        }
        if (stream is not null)
        {
            stream.Dispose();
        }
    }
}
