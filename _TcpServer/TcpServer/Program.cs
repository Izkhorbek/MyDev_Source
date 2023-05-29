using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    public class MulticastTcpSession : TcpSession
    {
        public MulticastTcpSession(TcpServer server) :base (server) {}

        protected override void OnReceived(byte[] buffer, long offset, long length)
        {
            string recmess = Encoding.ASCII.GetString(buffer, (int)offset, (int)length);
            Console.WriteLine("Received buffer - :" + recmess +"  "+ Id.ToString());

            TcpSession tcpSession = Server.FindSession(Id);
           
            if(tcpSession!=null)
            {
                _ = tcpSession.Send("Salom from Server to Client" + recmess);
            }
        }


    }

    public class MulticastServer: TcpServer
    {
        public MulticastServer(string IPAddress, int port) : base(IPAddress, port) { }

        protected override TcpSession CreateSession()
        {
            return new MulticastTcpSession(this);
        }
    }

    class Program
    {
        static void Main(string[] arg)
        {
            MulticastServer tcpServer = new MulticastServer("192.168.45.45", 12345);
            _ = tcpServer.Start();

            Console.ReadKey();
        }
    }
}
