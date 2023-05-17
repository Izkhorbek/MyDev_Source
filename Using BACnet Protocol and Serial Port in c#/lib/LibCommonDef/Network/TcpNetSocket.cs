using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibCommonDef.Network
{
    public class TcpNetSocket
    {
        #region Events
        public delegate void DelegateReceiveData(byte[] data, int length);
        public event DelegateReceiveData EventReceiveData;
        #endregion

        public bool IsConnected { get; set; }
        public IPEndPoint Endpoint { get; set; }

        private const int ReadSize = 1032;
        private const int ConnectTimeout = 10000;
        private TcpClient _baseSocket;
        private NetworkStream _baseStream;
        private byte[] _readBuffer = new byte[ReadSize];
        private bool _closing, _sending;
        private ConcurrentQueue<byte[]> _sendBuffer;

        #region Constructors
        public TcpNetSocket(string endpoint, int port)
        {
            if (port > 65535 || port < 1)
            {
                throw new Exception("Port number out of range. Valid range is 1-65535");
            }

            if (endpoint == null)
            {
                throw new Exception("Given endpoint is null.");
            }

            IPAddress address;

            if (!IPAddress.TryParse(endpoint, out address))
            {
                // -- Not an IP, try to get it as a dns name..
                IPAddress[] addresses = Dns.GetHostAddresses(endpoint);

                if (addresses == null || addresses.Length == 0)
                {
                    throw new Exception("Given endpoint is not a valid IP address or hostname.");
                }

                address = addresses[0];
            }

            Endpoint = new IPEndPoint(address, port);
            _baseSocket = new TcpClient();

            _sendBuffer = new ConcurrentQueue<byte[]>();
        }

        public TcpNetSocket(TcpClient client)
        {
            if (!client.Connected)
            {
                throw new Exception("Socket is not connected!");
            }

            _baseSocket = client;
            _baseStream = client.GetStream();
            Endpoint = new IPEndPoint(((IPEndPoint)client.Client.RemoteEndPoint).Address, ((IPEndPoint)client.Client.LocalEndPoint).Port);
            IsConnected = true;
            _sendBuffer = new ConcurrentQueue<byte[]>();
            _baseStream.BeginRead(_readBuffer, 0, ReadSize, ReadComplete, null); // -- Begin reading data
        }

        public TcpNetSocket()
        {
            _sendBuffer = new ConcurrentQueue<byte[]>();
        }
        #endregion

        #region Public Methods
        public Socket GetSocket()
        {
            return _baseSocket.Client;
        }

        public void Accept(TcpClient client)
        {
            if (!client.Connected)
            {
                throw new Exception("Socket is not connected!");
            }

            _baseSocket = client;
            _baseStream = client.GetStream();
            Endpoint = new IPEndPoint(((IPEndPoint)client.Client.RemoteEndPoint).Address, ((IPEndPoint)client.Client.LocalEndPoint).Port);
            IsConnected = true;
            _sendBuffer = new ConcurrentQueue<byte[]>();

            try
            {
                _baseStream.BeginRead(_readBuffer, 0, ReadSize, ReadComplete, null); // -- Begin reading data
            }
            catch (Exception e)
            {
                Disconnect($"Socket Disconnected {e.Message}");
            }
        }

        public void Connect()
        {
            if (Endpoint == null)
            {
                throw new Exception("No connection information provided.");
            }

            if (IsConnected)
                Disconnect("Connect() Called.");

            IAsyncResult handle = _baseSocket.BeginConnect(Endpoint.Address, Endpoint.Port, ConnectComplete, null);

            if (!handle.AsyncWaitHandle.WaitOne(ConnectTimeout))
                return;
        }

        public void Disconnect(string reason)
        {
            if (!IsConnected || _closing)
            {
                return;
            }

            _closing = true;
            var counter = 2000;

            while (_sending)
            {
                counter -= 1;

                if (counter <= 0)
                    break;

                Thread.Sleep(1);
            }

            _sendBuffer = new ConcurrentQueue<byte[]>();
            _readBuffer = new byte[ReadSize];

            _baseStream.Close();
            _baseSocket.Close();

            IsConnected = false;

            _closing = false;

            //ControlApp.Instance.SetInternalMessage(reason, MsgType.WARNING);
        }

        public async void Send(byte[] data)
        {
            _sendBuffer.Enqueue(data);

            if (_sending)
                return;

            _sending = true;
            await Task.Run(() => SendLoop());
        }
        #endregion

        #region Async Callbacks 
        private void SendLoop()
        { 
            while (_sendBuffer.Count > 0)
            {
                byte[] data;

                if (!_sendBuffer.TryDequeue(out data))
                {
                    _sending = false;
                    return;
                }

                try
                {
                    if (_baseStream != null && _baseStream.CanWrite)
                    {
                        try
                        {
                            _baseStream.Write(data, 0, data.Length);
                        }
                        catch
                        {
                            Disconnect("Could not send");
                        }
                    }
                    else
                    {
                        Disconnect("Socket closing");
                    }
                }
                catch
                {
                    Disconnect("Socket closing");
                }

                Thread.Sleep(1);
            }
              
            _sending = false;
        }

        private void DataSent(IAsyncResult ar)
        {
            try
            {
                _baseStream.EndWrite(ar);
            }
            catch
            {
                Disconnect("Socket closing");
            }
        }

        private void ConnectComplete(IAsyncResult ar)
        {
            try
            {
                _baseSocket.EndConnect(ar);
                IsConnected = true;
                _baseStream = _baseSocket.GetStream();
                _baseStream.BeginRead(_readBuffer, 0, ReadSize, ReadComplete, null);
            }
            catch (SocketException)
            {
                Disconnect("Socket Exception occured.");
                return;
            }
        }

        private void ReadComplete(IAsyncResult ar)
        {
            int received;

            try
            {
                received = _baseStream.EndRead(ar);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            catch (IOException e)
            {
                if (e.InnerException != null)
                    Disconnect("Socket exception occured: " + e.InnerException.HResult);
                else
                    Disconnect("Socket Exception occured.");

                return;
            }

            if (received == 0)
            {
                Disconnect("Connection closed by remote host.");
                return;
            }

            var newMem = new byte[received];
            Buffer.BlockCopy(_readBuffer, 0, newMem, 0, received);

            EventReceiveData?.Invoke(newMem, received);

            try
            {
                if (!_closing)
                    _baseStream.BeginRead(_readBuffer, 0, ReadSize, ReadComplete, null);
            }
            catch
            {
                Disconnect("Socket closing");
            }
        }
        #endregion

        #region Checksum
        static public ushort ComputeChecksum(byte[] payLoad)
        {
            uint xsum = 0;
            ushort shortval = 0, hiword = 0, loword = 0;

            for (int i = 0; i < payLoad.Length / 2; i++)
            {
                hiword = (ushort)(((ushort)payLoad[i * 2]) << 8);
                loword = (ushort)payLoad[(i * 2) + 1];
                shortval = (ushort)(hiword | loword);
                xsum = xsum + (uint)shortval;
            }

            if ((payLoad.Length % 2) != 0)
            {
                xsum += (uint)payLoad[payLoad.Length - 1];
            }

            xsum = ((xsum >> 16) + (xsum & 0xFFFF));
            xsum = (xsum + (xsum >> 16));
            shortval = (ushort)(~xsum);

            return shortval;
        }
        #endregion

        public bool IsClientConnected
        {
            get
            {
                try
                {
                    if (_baseSocket != null && _baseSocket.Client != null && _baseSocket.Client.Connected)
                    {
                        if ((_baseSocket.Client.Poll(0, SelectMode.SelectRead)) && (!_baseSocket.Client.Poll(0, SelectMode.SelectError)))
                        {
                            byte[] buffer = new byte[1];
                            if (_baseSocket.Client.Receive(buffer, SocketFlags.Peek) == 0)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
