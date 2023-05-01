using System;
using System.Collections.Generic;
using System.Net;
using System.Text;


namespace TcpServer
{
    public class TcpServer : IDisposable
    {
        /// <summary>
        /// Initialize TCP server with given TCP address and port number
        /// </summary>
        /// <param name = "address"> IP Address </param>
        /// <param port  = "port"> Port Number </param>
        public TcpServer(IPAddress address, int port) : this(new IPEndPoint(address, port)) {}

        /// <summary>
        /// initialize TCP server with given IP address and port number
        /// </summary>
        ///<param name="address"> TCP IP Address</param>
        ///<param port="port " > Port Number </param>
        public TcpServer(string address, int port) : this(new IPEndPoint(IPAddress.Parse(address), port)) { }

        /// <summary>
        /// Initialize TCP server with given DNS endpoint
        /// </summary>
        ///<param name="endpoint"> DNS endpoint </param>
        public TcpServer(DnsEndPoint endpoint) : this(endpoint as EndPoint, endpoint.Host, endpoint.Port) { }

        /// <summary>
        /// Initialize TCP server with given IP endpoint
        /// </summary>
        /// <parama name  ="endpoint "> IP Endpoint</parama>
        public TcpServer(IPEndPoint endpoint) : this(endpoint as EndPoint, endpoint.Address.ToString(), endpoint.Port) { }

        /// <summary>
        /// Initialize TCP with given endpoint, address and point
        /// </summary>
        /// <param name="endpoint"> endpoint</param>
        /// <param name="address"> Server address</param>
        /// <param name="point"> point</param>
        public TcpServer(EndPoint endpoint, string address, int port) 
        {
            Id = Guid.NewGuid();
            Address = address;
            Port = port;
            Endpoint = endpoint;
        }

        /// <summary>
        /// Server ID 
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// TCP Server address
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Tcp Server Port
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Endpoint
        /// </summary>
        public EndPoint Endpoint { get; private set; }
        

        #region IDispose Implementation
        /// <summary>
        /// Disposed flag
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Accepter socket disposed flag
        /// </summary>

        public bool IsSocketDisposed { get; private set; } = true;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposingManagedResources)
        {
            // The idea here is that Dispose(Boolean) knows whether it is
            // being called to do explicit cleanup (the Boolean is true)
            // versus being called due to a garbage collection (the Boolean
            // is false). This distinction is useful because, when being
            // disposed explicitly, the Dispose(Boolean) method can safely
            // execute code using reference type fields that refer to other
            // objects knowing for sure that these other objects have not been
            // finalized or disposed of yet. When the Boolean is false,
            // the Dispose(Boolean) method should not execute code that
            // refer to reference type fields because those objects may
            // have already been finalized."

            if(!IsDisposed)
            {
                if(disposingManagedResources)
                {
                    // Dispose managed resources here..
                }

                // Dispose unmanaged resoucerse here...

                // Set large fields to null here

                // Mark as disposed
                IsDisposed = true;
            }
        }

        #endregion
    }


}
