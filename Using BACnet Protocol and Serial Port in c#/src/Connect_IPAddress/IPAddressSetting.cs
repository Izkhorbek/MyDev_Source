using System;
using System.Collections.Generic;
using System.Text;

namespace Subway_BACnet.Connect_IPAddress
{
    public class Incheon_Trans_Subway
    {
        public string IPAddress;
        public int Port;
    }

    public class HuenSoft_Server
    {
        public string IPAddress;
        public int Port;
    }

    public class JBT_Server
    {
        public string IPAddress;
        public int Port;
    }

    public class SerialPortAdr
    {
        public string PortName { get; set; }
        public int Baudrate { get; set; }
    }
}
