using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    public class TcpSession : IDisposable
    {
        /// <summary>
        /// Tcp Server 
        /// </summary>
        public TcpServer Server { get; }

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
        internal async Task Connect(TcpClient tcpClient)
        {

            Client = tcpClient;

            IsConnected = true;

            await TryAsyncReceive();
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
                    Console.WriteLine("Session is reading buffer from id:" + Id.ToString());
                    while (Client.Connected)
                    {
                        ReceivedBuffer = new byte[OptionReceiveBufferSize];

                        int bytesRead = await stream.ReadAsync(ReceivedBuffer, 0, ReceivedBuffer.Length);

                        if (bytesRead == 0)
                        {
                            Console.WriteLine("bytesRead is Zero " + Id.ToString());
                            break;
                        }

                        OnReceived(ReceivedBuffer, 0, ReceivedBuffer.Length);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Client is disconnected, Id:" + this.Id.ToString() + "\n"+ex.Message);

                _receiving = false;
                IsConnected = false;

                // Dispose here

                //unregister the session
                Server.UnregisterSession(Id);
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
        /// This is called when buffer was received from the client
        /// </remarks>
        protected virtual void OnReceived(byte[] buffer, long offset, long length) { }

        /// <summary>
        /// Send the text to cleint
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public virtual async Task Send(string text) => await Send(Encoding.ASCII.GetBytes(text), 0, (long)Encoding.ASCII.GetBytes(text).Length);

        /// <summary>
        /// Send data to the client (asynchronous)
        /// </summary>
        /// <param name="buffer">Buffer to send as a span of bytes</param>
        /// <returns>'true' if the data was successfully sent, 'false' if the session is not connected</returns>
        public async Task Send(byte[] buffer, int offset, long size)
        {
            if (!IsConnected)
                return ;

            if (buffer.Length == 0)
                return;

            SendBuffer = new byte[size];
            SendBuffer = buffer[offset .. (int)size];

            await TryAsyncSend();
        }

        private async Task TryAsyncSend()
        {
            if (!IsConnected)
                return;

            try
            {
                using (NetworkStream stream = Client.GetStream())
                {
                    while (Client.Connected)
                    {
                       await stream.WriteAsync(SendBuffer, 0, SendBuffer.Length);
                    }

                    SendBuffer = null;
                }
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
        }
    }
}
