using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using LibCommonDef;
using System.Threading.Tasks;

namespace Incheon_BACnet.Connect_IPAddress
{
   public class SerialPortConnect
    {
        public string _portName { get; set; }
        public int _baudrate { get; set; }
        private StopBits _stopBits { get; set; }
        private int _dataBits { get; set; }
        private bool _DtrEnable { get; set; }
        private bool _RtsEnable { get; set; }
        private int _EnumHandshake { get; set; }
        public bool isConnected { get; set; }

        private byte[] sendBuffer = new byte[1024]; 
        private SerialPort serialPort; 
        public SerialPortConnect(string portname, int baudrate, int handshake, int dtrenable, int rtsenable)
        {
            this._portName = portname;
            this._baudrate = baudrate;
            this._stopBits   = StopBits.One;
            this._dataBits   = 8;
            this._DtrEnable  = (dtrenable==0)?false:true;
            this._RtsEnable  = (rtsenable==0)?false:true;
            this._EnumHandshake = handshake;
    }
        public bool Connect()   
        {
            serialPort = new SerialPort(this._portName, this._baudrate);
            serialPort.StopBits = _stopBits;
            serialPort.DataBits = _dataBits;
            serialPort.DtrEnable = _DtrEnable;
            serialPort.RtsEnable = _RtsEnable;
            serialPort.Handshake = (Handshake)_EnumHandshake;
            serialPort.Parity = Parity.None;
            try
            {
                serialPort.Open();
                if(serialPort.IsOpen)
                {
                    this.isConnected = true;
                    LogWriter.Instance.Write($"Serial Port {this._portName}:{ this._baudrate} is open");
                }
            }
            catch
            {
                this.isConnected = false;
                LogWriter.Instance.Write($"Serial Port {this._portName}:{ this._baudrate} isn't open");
                return false;
            }
            return true;
        }
        public async Task Write(byte[] data)
        {
            if(serialPort.IsOpen)
            {
                try
                {
                    Array.Clear(sendBuffer, 0, sendBuffer.Length);
                    Array.Copy(data, sendBuffer, data.Length);

                    await serialPort.BaseStream.WriteAsync(sendBuffer.AsMemory(0, data.Length));

                    serialPort.DataReceived += SerialPort_DataReceived;

                    bool arriv = serialPort.CtsHolding;
                    if (!arriv) { LogWriter.Instance.Write("not ClearToSend"); }
                    data = null;
                }
                catch(ArgumentException ex)
                {
                    LogWriter.Instance.Write(ex.Message.ToString());
                }
            }
            else
            {
                LogWriter.Instance.Write("SerialPort isn't open!");
            }
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();

            LogWriter.Instance.Write("NPort Receievd Exception: " + e.ToString());
        }
        public void Close()
        {
            if(serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                }
                catch(Exception ex)
                {
                    LogWriter.Instance.Write(ex.Message.ToString());
                }
            }

        }
    }
}
