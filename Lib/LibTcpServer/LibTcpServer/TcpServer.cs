using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//Example
// Example for Multicast Tcp Server
//public class MulticastTcpSession : TcpSession
//{
//    public MulticastTcpSession(TcpServer server) :base (server) {}

//    protected override async Task AsyncOnReceived(byte[] buffer, long offset, long length)
//    {
//         Processing processing = new Processing();

//         bool result = await processing.RequestfromDB(this, buffer, offset, length);
//    }
//}

//public class MulticastServer: TcpServer
//{
//    public MulticastServer(string IPAddress, int port) : base(IPAddress, port) { }
//    public MulticastServer(int port) : base(port) { }

//    protected override TcpSession CreateSession()
//    {
//        return new MulticastTcpSession(this);
//    }
//}

namespace LibTcpServer
{
    public class TcpServer :IDisposable
    {
        /// <summary>
        /// Server Id
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Tcp Server Tcp IpAddress
        /// </summary>
        public string IpAddress { get; }
        /// <summary>
        /// Tcp Server Port
        /// </summary>
        public int Port { get; }
        
        /// <summary>
        /// Endpoint
        /// </summary>
        public EndPoint EndPoint { get; private set; }

        /// <summary>
        /// Is server started?
        /// </summary>
        public bool IsStarted { get; private set; }

        /// <summary>
        /// Is the server accepting new clients?
        /// </summary>
        public bool IsAccepting { get; private set; }

        /// <summary>
        /// Server acceptor
        /// </summary>
        private TcpListener _acceptorClient;

        /// <summary>
        /// Invoke Server user from receiving data
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public delegate void BufferReceived(byte[] bytes, int offset, int length);

        /// <summary>
        /// Event 
        /// </summary>
        public event BufferReceived eventBufferReceived;
        
        /// <summary>
        /// Client count that is connected to server;
        /// </summary>
        private int Client_Count { get; set; } = 0;

        /// <summary>
        /// Create Socket 
        /// </summary>
        /// <returns></returns>
        protected  TcpListener CreateListener()
        {
            return new TcpListener(IPAddress.Parse(IpAddress), Port);
        }
        
        /// <summary>
        /// Tpc Server requests a address and port number
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public TcpServer(string address, int port) : this ( new IPEndPoint( IPAddress.Parse(address), port), address, port) {}

        /// <summary>
        /// Tcp Server request a port number
        /// </summary>
        /// <param name="port"></param>
        public TcpServer(int port) : this(IPAddress.Any.ToString(), port) {}

        /// <summary>
        /// Tcp Server requests endpoint, ipaddress and port number
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public TcpServer(IPEndPoint endpoint, string address, int port)
        {
            Id = Guid.NewGuid();
            IpAddress = address;
            Port = port;
            EndPoint = endpoint; 
        }
        
        /// <summary>
        /// Start Server
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            Debug.Assert(!IsStarted, "TCPServer is already started!");

            if (IsStarted)
                return;
            // Initialize a client acceptor 
            _acceptorClient = null;

            try
            {
                //Create a new accepter
                _acceptorClient = CreateListener();

                //Bind the acceptor socket to the endpoint
                _acceptorClient.Start();

                Console.WriteLine("Server is started. \nListening for incoming connections...");
                IsAccepting = true;

                while (IsAccepting)
                {
                    TcpClient tcpClient = await _acceptorClient.AcceptTcpClientAsync();

                    _ = ProcessAccept(tcpClient);
                    Console.WriteLine($"Client {Client_Count.ToString()} : " + tcpClient.Client.RemoteEndPoint.ToString());
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                IsAccepting = false;
            }
        }

        /// <summary>
        /// Stop Tcp Server
        /// </summary>
        public void Stop()
        {
            Debug.Assert(IsStarted, "TCP server isn't started!");
            if (!IsStarted)
                return;

            _acceptorClient.Stop();
            IsStarted = false;
            Dispose();
        }
        /// <summary>
        /// Start to Accept clients
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <returns></returns>
        private async Task ProcessAccept(TcpClient tcpClient)
        {
            try
            {
                //Create a session
                var session = CreateSession();

                // Register the session
                RegisterSession(session);

                // Start to Connect;
                await session.Connect(tcpClient);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
                
        }

        #region  Session Menegment
        
        /// <summary>
        /// Add the sessions with Id to Container
        /// </summary>
        protected readonly ConcurrentDictionary<Guid, TcpSession> Sessions = new ConcurrentDictionary<Guid, TcpSession>();
        
        /// <summary>
        /// Create a new session
        /// </summary>
        /// <returns></returns>
        protected virtual TcpSession CreateSession()
        {
            return new TcpSession(this);
        }

        /// <summary>
        /// Register the session
        /// </summary>
        /// <param name="session"></param>
        internal void RegisterSession(TcpSession session)
        {
            // Register a new session
            if(Sessions.TryAdd(session.Id, session))
               Client_Count++;
        }

        /// <summary>
        /// Find the session fro, container
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public TcpSession FindSession(Guid guid)
        {
            return Sessions.TryGetValue(guid, out TcpSession session) ? session : null;
        }

        /// <summary>
        /// Unregister the session from container
        /// </summary>
        /// <param name="guid"></param>
        public void UnregisterSession(Guid guid)
        {
            if(Sessions.TryRemove(guid, out TcpSession _))
                Client_Count--;
        }

        public virtual bool DisconnectAll()
        {
            if(!IsStarted)
                return false;

            // Disconnect all sessions
            foreach (var session in Sessions.Values)
                session.Disconnect();
            
            return true;
        }
        #endregion

        #region Option setting
        /// <summary>
        /// This option will set the listening socket's blacklog size
        /// </summary>
        public int OptionAcceptorBackLog { get; set; } = 1024;

        /// <summary>
        /// Option: Receive buffer size from client
        /// </summary>
        public int OptionReceiveBufferSize { get; set; } = 8192;
        /// <summary>
        /// Option: Send buffer size to client
        /// </summary>
        public int OptionSendBufferSize { get; set; } = 8192;
        #endregion

        #region  Server handlers

        protected virtual void OnDataReceived(byte[] bytes, int offset, int length) { eventBufferReceived?.Invoke(bytes, offset, length); }
        
        /// <summary>
        /// Handle session connection notification
        /// </summary>
        /// <param name="session"></param>
        /// <remarks>
        /// Notify if A session is created and connected 
        /// </remarks>
        protected virtual void OnSessionConnected(TcpSession session) { }

        /// <summary>
        /// Handle session disconnection notification
        /// </summary>
        /// <param name="session"></param>
        /// <remarks>
        /// Notify if A session is disconnected 
        /// </remarks>
        protected virtual void OnSessionDisconnected(TcpSession session) { }

        internal void OnSessionConnectedInternal(TcpSession session) { OnSessionConnected(session); }
        internal void OnSessionDisconnectedInternal(TcpSession session) { OnSessionDisconnected(session); }
        internal void OnSessionOnDataReceivedInternal(byte[] bytes, int offset, int length) { OnDataReceived(bytes, offset, length); }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
