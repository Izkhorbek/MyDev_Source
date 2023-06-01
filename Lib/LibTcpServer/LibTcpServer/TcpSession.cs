using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LibTcpServer
{
    public class TcpSession : IDisposable
    {
        /// <summary>
        /// Tcp Server 
        /// </summary>
        private TcpServer Server { get; }

        /// <summary>
        /// Set tcpClient to Session
        /// </summary>
        public TcpClient Client { get; private set; }

        /// <summary>
        /// Session ID
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Is the session connected?
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Check if buffer from a client is receiving or not?
        /// </summary>
        private bool _receiving;

        /// <summary>
        /// Received buffer from client;
        /// </summary>
        private byte[] ReceivedBuffer;

        /// <summary>
        /// Send buffer to client
        /// </summary>
        private byte[] SendBuffer;

        /// <summary>
        /// Initialize the session wiht given server
        /// </summary>
        /// <param name="server"></param>
        public TcpSession(TcpServer server)
        {
            Id = Guid.NewGuid();
            Server = server;
            OptionReceiveBufferSize = server.OptionReceiveBufferSize;
            OptionSendBufferSize = server.OptionSendBufferSize;
        }

        /// <summary>
        /// Connect the session with tcpclient
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <returns></returns>
        internal async Task Connect(TcpClient tcpClient)
        {
            Client = tcpClient;

            IsConnected = true;

            //Inform to server the client is connected
            Server.OnSessionConnectedInternal(this);

            await TryAsyncReceive();
        }

        /// <summary>
        /// Disconnect the session 
        /// </summary>
        /// <returns></returns>
        public virtual bool Disconnect()
        {
            if (!IsConnected)
                return false;

            // Unregister session
            Server.UnregisterSession(Id);

            //Close the client connection
            Client.Close();
            
            // is CLient connected ?
            IsConnected = false;

            _receiving = false;

            Client.Dispose();
            Dispose();
            return true;
        }

        #region Send / Receive management

        /// <summary>
        /// Try Async receive buffer from client
        /// </summary>
        /// <returns></returns>
        private async Task TryAsyncReceive()
        {
            if (_receiving)
                return;

            if (!IsConnected)
                return;

            try
            {
                _receiving = true;

                using (NetworkStream stream = Client.GetStream())
                {
                    while (Client.Connected)
                    {
                        ReceivedBuffer = new byte[OptionReceiveBufferSize];

                        int bytesRead = await stream.ReadAsync(ReceivedBuffer, 0, ReceivedBuffer.Length);

                        if (bytesRead == 0)
                        {
                            Console.WriteLine("bytesRead is Zero " + Id.ToString());
                            break;
                        }
                        
                        _ = AsyncOnReceived(ReceivedBuffer, 0, bytesRead);
                        OnReceived(ReceivedBuffer, 0, bytesRead);
                        Server.OnSessionOnDataReceivedInternal(ReceivedBuffer, 0, bytesRead);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Client is disconnected, Id:" + this.Id.ToString() + "\n"+ex.Message);

                //unregister the session
                Server.UnregisterSession(Id);

                _receiving = false;
                IsConnected = false;

                //Inform to server the client is disconnected
                Server.OnSessionDisconnectedInternal(this);

                Client.Dispose();
                Dispose();
            }
            finally
            {
                Client.Close();
            }

        }

        /// <summary>
        /// Handle Received Buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <remarks>
        /// async: This is called asynchrounly when buffer was received from the client
        /// </remarks>
        protected virtual async Task AsyncOnReceived(byte[] buffer, long offset, long length) { await Task.CompletedTask; }

        /// <summary>
        /// Handle Received buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <remarks>
        /// void: This is called when buffer was received from the client
        /// </remarks>
        protected virtual void OnReceived(byte[] buffer, long offset, long length) {}
        /// <summary>
        /// Send the text to cleint
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public  void Send(string text) => Send(Encoding.ASCII.GetBytes(text), 0, (long)Encoding.ASCII.GetBytes(text).Length);

        public void Send(byte[] bytes) => Send(bytes, 0, bytes.Length);

        /// <summary>
        /// Send data to the client (asynchronous)
        /// </summary>
        /// <param name="buffer">Buffer to send as a span of bytes</param>
        /// <returns>'true' if the data was successfully sent, 'false' if the session is not connected</returns>
        public void Send(byte[] buffer, int offset, long size)
        {
            if (buffer.Length == 0)
                return;

            SendBuffer = new byte[size];
            buffer.CopyTo(SendBuffer, size);

           _ = TryAsyncSend();
        }

        /// <summary>
        /// Try to async send buffer to the clients
        /// </summary>
        /// <returns></returns>
        private async Task TryAsyncSend()
        {
            if (!IsConnected)
                return;

            try
            {
                NetworkStream stream = Client.GetStream();
                    if (Client.Connected)
                    {
                       await stream.WriteAsync(SendBuffer, 0, SendBuffer.Length);
                    }
                    else
                    {
                        Console.WriteLine("Client is disconnected." + Id.ToString());
                        Server?.OnSessionDisconnectedInternal(this);
                        IsConnected = false;
                        return;
                    }

                    SendBuffer = null;
            }
            catch (IOException ex)
            {
                Console.WriteLine(this.Id.ToString() + " - "+ ex.Message);
            }
        }

        #endregion

        #region Option Setting
        /// <summary>
        /// Option: Receive buffer size from client
        /// </summary>
        public int OptionReceiveBufferSize { get; set; } = 1024;
        
        /// <summary>
        /// Option: Send buffer size to client
        /// </summary>
        public int OptionSendBufferSize { get; set; }
        #endregion

        public void Dispose()
        {
            Client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
