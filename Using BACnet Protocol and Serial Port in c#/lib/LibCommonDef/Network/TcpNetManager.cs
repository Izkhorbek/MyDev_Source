using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Net.Sockets;
//using System.Text.Json;
//using Newtonsoft.Json;

namespace LibCommonDef.Network
{
    public class TcpNetManager
    {
        public delegate void ReceivedData(byte[] data, int length);
        public event ReceivedData EventReceivedData;

        public static readonly short HEADERSIZE = 8;
        public string connectedIP = null;
        public int connectedPort = 0;

        private TcpNetSocket tcpClient = null;

        public TcpNetManager(string ipAdress, int port)
        {
            this.connectedIP = ipAdress;
            this.connectedPort = port;

            tcpClient = null;
            CreateClient();
        }

        public bool IsClientConnected => tcpClient == null ? false : tcpClient.IsClientConnected;

        public void CreateClient()
        {
            if (tcpClient != null)
                tcpClient.EventReceiveData -= ReceivedProcess;

            tcpClient = null;
            tcpClient = new TcpNetSocket(connectedIP, connectedPort);
            if (tcpClient != null)
            {
                tcpClient.Connect();
            }
            tcpClient.EventReceiveData += ReceivedProcess;
        }

        public void Send(HeaderData header, byte[] data)
        {
            if (tcpClient == null) return;

            header.m_usDataSize = (ushort)data.Length;
            byte[] headerBytes = GetBytes(header);
            //byte[] sendingData = Combine(headerBytes, data);
            byte[] sendingData = new byte[headerBytes.Length + data.Length];
            headerBytes.CopyTo(sendingData, 0);
            data.CopyTo(sendingData, headerBytes.Length);

            tcpClient.Send(sendingData);
        }

        public void Send(TcpHeaderData  header, byte[] data)
        {
            if (tcpClient == null) return;
            byte[] headerBytes = GetBytes(header);
            //byte[] sendingData = Combine(headerBytes, data);

            byte[] sendingData = new byte[headerBytes.Length + data.Length];
            headerBytes.CopyTo(sendingData, 0);
            data.CopyTo(sendingData, headerBytes.Length);

            tcpClient.Send(sendingData);
        }
        public void Send(HeaderData header, FlightPlaneInfo data)
        {
            Send(header, GetBytes(data));
        }

        public void Send(HeaderData header, ProhibitedAreInfo data)
        {
            Send(header, GetBytes(data));
        }

        public void Send(HeaderData header, AircraftData data)
        {
            Send(header, GetBytes(data));
        }

        public void Send(HeaderData header, CategoryData data)
        {
            Send(header, GetBytes(data));
        }

        public void Send(byte[] bytes)
        { 
            if (tcpClient == null) return;
            if (bytes.Length == 0) return;
            
            tcpClient.Send(bytes);
        }
        public void ReceivedProcess(byte[] data, int length)
        {
            this.EventReceivedData?.Invoke(data, length);
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }

        public static byte[] GetBytes(object obj)
        {
            int size = Marshal.SizeOf(obj);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public static Temp_Obj GetObjFromBytes<Temp_Obj>(byte[] arr, int startIndex) where Temp_Obj : new()
        {
            var str = new Temp_Obj();

            int size = Marshal.SizeOf(str);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, startIndex, ptr, size);

            str = (Temp_Obj)Marshal.PtrToStructure(ptr, str.GetType());
            Marshal.FreeHGlobal(ptr);

            return str;
        }

    }
}
