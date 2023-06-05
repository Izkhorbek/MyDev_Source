using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using LibCommonDef;

namespace Subway_BACnet
{
    public delegate void SendData(List<byte> list);
    public class SendThread
    {
        public event SendData EventSendData;
        private string _subwayLineNumber { get; set; }
        private Thread _thread;
        public SettingInfo settinginfo { get; set; }
        private Dictionary<List<Device>, int> _deviceList { get; set; }
        
       
        public SendThread(Dictionary<List<Device>, int> dList)
        {
            _deviceList = dList;
        }

        public void Start(string threadName)
        {
            _thread = new Thread(new ThreadStart(DoJob));
            _thread.IsBackground = true;
            _thread.Name = threadName;
            _subwayLineNumber = settinginfo.LineNumber < 10 ? "0" + settinginfo.LineNumber.ToString() : settinginfo.LineNumber.ToString();

            _thread.Start();
        }

        void DoJob()
        {
            if (settinginfo.SendBufferSize == 0)
            {
                Console.WriteLine("Error sendpacketSize");
                return;
            }

            ReadDatas readData = new ReadDatas();

            while (true)
            {
                byte _startbyte = 0x02, _endbyte = 0x03; byte _endbyteFF = 0xff;

                foreach (KeyValuePair<List<Device>, int> dList in _deviceList)
                {
                    int devCount = 0;
                    List<byte> SendDatList = new List<byte>();
                  
                    byte[] _commandbyte = new byte[4];
                    _commandbyte = Encoding.ASCII.GetBytes("ELEC");

                    byte[] _lineNo = new byte[2];
                    _lineNo = Encoding.ASCII.GetBytes(_subwayLineNumber);

                    byte[] _stnNo = new byte[4];
                    _stnNo = Encoding.ASCII.GetBytes(("I"+(dList.Value.ToString())));

                    byte[] devID = new byte[25];
                    byte devValue = new byte();
                    byte[] byteDate = new byte[14];

                    foreach (Device device in dList.Key)
                    {

                        string[]? RetValue = new string[2];
                        RetValue[0] = string.Empty;
                        RetValue[1] = string.Empty;

                        RetValue = readData.ReadBACnetData(device.id, device.infoList.pointInstance);

                        Thread.Sleep(settinginfo.DeviceRespondWaitTime);

                        if (RetValue[0] != null && RetValue[1] != null && RetValue[0] != "-1" && RetValue[1]!="255")
                        {
                            Console.WriteLine(DateTime.Now.ToString("dd:HH:mm:ss ") + _thread.Name+ ": " + RetValue[0] + ": " + RetValue[1]);
                            devCount++;

                            //1 Device ID 
                            try
                            { 
                                Encoding.ASCII.GetBytes(RetValue[0]).CopyTo(devID, 0);
                            }
                            catch
                            {
                                LogWriter.Instance.Write($"Error: Device Name: {Convert.ToString(RetValue[0])}, Val: {Convert.ToString(RetValue[1])} ");
                                return;
                            }
                            //2 Device Value 
                            try
                            {
                                devValue = Byte.Parse(RetValue[1]);
                            }
                            catch (Exception)
                            {
                                LogWriter.Instance.Write($"Error: Device Name: {Convert.ToString(RetValue[0])}, Val: {Convert.ToString(RetValue[1])} ");
                                devValue = 0;
                                return;
                            }
                            
                            //3 DateTime
                            string Date = DateTime.Now.ToString("yyyyMMddHHmmss");
                            try
                            {
                                Encoding.ASCII.GetBytes(Date).CopyTo(byteDate, 0);
                            }
                            catch
                            {
                                return;
                            }

                            SendDatList.AddRange(devID);
                            SendDatList.Add(devValue);
                            SendDatList.AddRange(byteDate);
                        }
                        
                        if (SendDatList.Count >= settinginfo.SendBufferSize)
                        {
                            byte[] _facilCount = new byte[2];
                            _facilCount = BitConverter.GetBytes(((short)devCount));
                            Array.Reverse(_facilCount, 0, _facilCount.Length);


                            byte[] _lengthbyte = new byte[2];
                            _lengthbyte = BitConverter.GetBytes((short)(SendDatList.Count + 10));
                            Array.Reverse(_lengthbyte, 0, _lengthbyte.Length);

                            SendDatList.Insert(0, _startbyte);
                            SendDatList.InsertRange(1, _commandbyte);
                            SendDatList.InsertRange(5, _lengthbyte);
                            SendDatList.InsertRange(7, _lineNo);
                            SendDatList.InsertRange(9, _stnNo);
                            SendDatList.InsertRange(13, _facilCount);

                            SendDatList.Insert(SendDatList.Count, _endbyteFF);
                            SendDatList.Insert(SendDatList.Count, _endbyte);

                            EventSendData?.Invoke(SendDatList);

                            devCount = 0;
                            SendDatList = new List<byte>();
                        }
                    }
                     

                    if (SendDatList.Count > 0 && SendDatList.Count < settinginfo.SendBufferSize)
                    {
                        byte[] _facilCount = new byte[2];
                        _facilCount = BitConverter.GetBytes(((short)devCount));
                        Array.Reverse(_facilCount, 0, _facilCount.Length);


                        byte[] _lengthbyte = new byte[2];
                        _lengthbyte = BitConverter.GetBytes((short)(SendDatList.Count + 10));
                        Array.Reverse(_lengthbyte, 0, _lengthbyte.Length);

                        SendDatList.Insert(0, _startbyte);
                        SendDatList.InsertRange(1, _commandbyte);
                        SendDatList.InsertRange(5, _lengthbyte);
                        SendDatList.InsertRange(7, _lineNo);
                        SendDatList.InsertRange(9, _stnNo);
                        SendDatList.InsertRange(13, _facilCount);

                        SendDatList.Insert(SendDatList.Count, _endbyteFF);
                        SendDatList.Insert(SendDatList.Count, _endbyte);

                        EventSendData?.Invoke(SendDatList);

                    }

                }

                Thread.Sleep(settinginfo.BACnetRequestUpdateTime);
            }
        }
    }
}
