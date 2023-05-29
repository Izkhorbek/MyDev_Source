using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpServer
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
        /// Create Socket 
        /// </summary>
        /// <returns></returns>
        protected  TcpListener CreateListener()
        {
            return new TcpListener(IPAddress.Parse(IpAddress), Port);
        }
        public TcpServer(string address, int port) : this ( new IPEndPoint( IPAddress.Parse(address), port), address, port) {}
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
                        
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                IsAccepting = false;
            }
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
            Sessions.TryAdd(session.Id, session);
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
            Sessions.TryRemove(guid, out TcpSession _);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
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
        public int OptionReceiveBufferSize { get; set; } = 1024;
        /// <summary>
        /// Option: Send buffer size to client
        /// </summary>
        public int OptionSendBufferSize { get; set; } = 1024;
        #endregion

    }
}
