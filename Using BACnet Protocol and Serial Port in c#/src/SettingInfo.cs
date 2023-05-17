using System;
using System.Collections.Generic;
using System.Text;
using Incheon_BACnet.Connect_IPAddress;


namespace Incheon_BACnet
{
    public class SettingInfo
    {
        public int LineNumber { get; set; }
        public int UpdateTimeWhois { get; set; }
        public int BACnetRequestUpdateTime { get; set; }
        public int SendTimeSerialPortData { get; set; }
        public int DeviceRespondWaitTime { get; set; }
        public int SendBufferSize { get; set; }
        public int EnumHandshake { get; set; }
        public int DtrEnable { get; set; }
        public int RtsEnable { get; set; }
        
        public Incheon_Trans_Subway Incheon_Trans_Subway;
        public HuenSoft_Server HuenSoft_Server;
        public JBT_Server JBT_Server;
        public SerialPortAdr SerialPortAdr;

        //public bool IsOk()
        //{
        //    if ((Incheon_Trans_Subway == null) || string.IsNullOrEmpty(Incheon_Trans_Subway.IPAddress) || string.IsNullOrEmpty(Incheon_Trans_Subway.IPAddress)
        //            ) return false;

        //    return true;
        //}
    }
}
