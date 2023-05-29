using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NDesk.Options;

namespace TcpServer
{
    public class MulticastSession : TcpSession
    {
        public MulticastSession(TcpServer server) : base(server) {}

        protected override void OnConnected()
        {
            Console.WriteLine($"\nChat Tcp session with Id {Id} connected");

            //Send invite message
            string message = "Hello from Tcp chat!, Please send a message or `!` to disconnect the client";
            //SendAsync(message);
            SendAsync(message);
        }
        protected override void OnDisconnected()
        {            
            Console.WriteLine($"Chat TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
         
            Console.WriteLine("Incoming: " + message);

            // Multicast message to all connected sessions

            //Server.Multicast(message);
            TcpSession session = Server.FindSession(this.Id);
            string mesOK = string.Empty;
            if (message == "Client1")
                mesOK = "Client1";
            if (message == "Client2")
                mesOK = "Client2";
            session.SendAsync(mesOK);

            // If the buffer starts with '!' the disconnect the current session
            if (message == "!")
                Disconnect();
        }
        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP session caught an error with code {error}");
        }
    }

    public class MulticastServer: TcpServer
    {
        public MulticastServer(IPAddress address, int port) : base(address, port) {}

        protected override TcpSession CreateSession()
        {
            return new MulticastSession(this);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Server caught an error with code: {error.ToString()}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool help = false;
            int port = 2345;
            int messagesRate = 1000000;
            int messageSize = 32;

            var options = new OptionSet()
            {
                {"h|?|help", v=>help =v!=null },
                {"p|port=", v=>port = int.Parse(v) },
                {"m|messages=", v=>messagesRate = int.Parse(v)},
                {"s|size=", v=>messageSize = int.Parse(v)}
            };

            try
            {
                options.Parse(args);
            }
            catch(OptionException ex)
            {
                Console.WriteLine($"Common line error: {ex.Message}");
                Console.WriteLine("Try `--help` to get usage information.");
                return;
            }

            if(help)
            {
                Console.WriteLine("Usage:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            Console.WriteLine($"Server port: {port}");
            Console.WriteLine($"Messages rate: {messagesRate}");
            Console.WriteLine($"Message size: {messageSize}");

            Console.WriteLine();

            //Create a new echo server
            var server = new MulticastServer(IPAddress.Parse("192.168.45.45"), port);

            // Server.OptionNoDelay = true;
            server.OptionReuseAddress = true;


            //Start the server
            Console.WriteLine("Server starting...");
            server.Start();
            Console.WriteLine("Done!");


            ////Start the multicasting thread
            bool multicasting = true;
            //var multicaster = Task.Factory.StartNew(() =>
            //{
            //    //Prepare message to multicast
            //    byte[] message = new byte[messageSize];

            //    //Multicasting  loop
            //    while(multicasting)
            //    {
            //        var start = DateTime.UtcNow;
            //        //for (int i = 0; i < messagesRate; i++)
            //           // server.Multicast(message);
                    
            //        var end = DateTime.UtcNow;

            //        // Sleep for remaining time or yield
            //        var milliseconds = (int)(end - start).TotalMilliseconds;

            //        if (milliseconds < 1000)
            //            Thread.Sleep(1000 - milliseconds);
            //        else
            //            Thread.Yield();
            //    }   
            //});

            Console.Write("Press Enter to stop the server or `!` to restart the server...");

            //Perform the input
            while(true)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                //Restart the server
                if(line == "!")
                {
                    Console.Write("Server restarting...");
                    server.Restart();
                    Console.WriteLine("Done!");
                }
            }

            //Stop the multicasting thread
            multicasting = false;
          //  multicaster.Wait();

            //Stop the server
            Console.Write("Server stopping...");
            server.Stop();
            Console.WriteLine("Done!");
        }
    }
}
