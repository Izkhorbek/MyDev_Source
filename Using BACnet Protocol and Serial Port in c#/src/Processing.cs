using System;
using System.Collections.Generic;
using System.Threading;
using LibCommonDef;
using System.IO;
using System.Linq;
using System.IO.BACnet;
using Subway_BACnet.Connect_IPAddress;
using System.Collections.Concurrent;

namespace Subway_BACnet
{
    class Processing : SingletonObj<Processing>
    {
        //DeviceList(IpAddress and ObjectID) 
        public List<BacNode> DevicesList = new List<BacNode>();

        // BACnet Client Connection
        public BacnetClient bacnet_client;

        public bool[] _btcpConnected = new bool[7];

        public ReadBACnetAddressTable LoadProtocol; 

        // BACnet Info setting data
        SettingInfo settinginfo;

        public ConcurrentQueue<List<byte>> queueByteBuffer = new ConcurrentQueue<List<byte>>();

        public SerialPortConnect _serialPortConnect;

        SendBACnetDeviceData sendBACnetDeviceData = new SendBACnetDeviceData();

        public void Run(SettingInfo info)
        {
            settinginfo = info;

            // 1. Get_Path
            string _execute_path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            LoadProtocol = new ReadBACnetAddressTable(_execute_path);
            // 2. Load BACnet Protocol from(.csv);
            Console.WriteLine(LoadProtocol.Read() ? "Load setting BACnet Protocol" : "Check BACnet Protocol csv.file!");
           
            #region SerialPortConnection

            _serialPortConnect = new SerialPortConnect(settinginfo.SerialPortAdr.PortName, settinginfo.SerialPortAdr.Baudrate, settinginfo.EnumHandshake, settinginfo.DtrEnable, settinginfo.RtsEnable);
            _serialPortConnect.Connect();

            #endregion SerialPortConnection

            bacnet_client = new BacnetClient(new BacnetIpUdpProtocolTransport(settinginfo.Incheon_Trans_Subway.Port, false, false, 1472, settinginfo.Incheon_Trans_Subway.IPAddress));

            if (bacnet_client != null)
            {
                try
                {
                    bacnet_client.Start();
                    LogWriter.Instance.Write("BACnet UDP Connection is started !");
                    StartActivity();
                }
                catch (Exception  ex)
                {
                    LogWriter.Instance.Write(ex.Message);
                    LogWriter.Instance.Write("BACnet UDP Connection can't start... Please check the connection!");
                    return;
                }

                System.Timers.Timer WhoisTimer = new System.Timers.Timer();
                WhoisTimer.Interval = info.UpdateTimeWhois;
                WhoisTimer.AutoReset = true;
                WhoisTimer.Enabled = true;
                WhoisTimer.Elapsed += new System.Timers.ElapsedEventHandler(RequestWhoisfromDevices);
                WhoisTimer.Start();

                StartAllSubwayThread();

            }


            // Send Data 
            sendBACnetDeviceData.settingInfo = settinginfo;
            sendBACnetDeviceData.StartThread();

        }
        private void RequestWhoisfromDevices(object sender, System.Timers.ElapsedEventArgs e)
        {
             StartActivity();
        }
        public void StartActivity()
        {
            // Bacnet on UDP/IP/Ethernet
             bacnet_client.OnIam += Bacnet_client_OnIam;
             bacnet_client.WhoIs();
             LogWriter.Instance.Write("Whois is requested");
             Thread.Sleep(1000); // Wait a fiew time for WhoIs responses (managed in handler_OnIam)
        }

        private void Bacnet_client_OnIam(BacnetClient sender, BacnetAddress adr, uint deviceId, uint maxAPDU, BacnetSegmentations segmentation, ushort vendorId)
        {
            lock (DevicesList)
            {
                // Device already registred ?
                foreach (BacNode bn in DevicesList)
                    if (bn.getAdd(deviceId) != null) return;   // Yes

                // Not already in the list
                DevicesList.Add(new BacNode(adr, deviceId));   // add it
            }

            LogWriter.Instance.Write("BACnet Device Count: " + DevicesList.Count.ToString());
        }

        private void StartAllSubwayThread()
        {
            LogWriter.Instance.Write("Start threads...");
            RequestDeviceInfo1();     // Device 8211 ~ 8263
            RequestDeviceInfo2();    // Device 8261 ~ 8302 
            RequestDeviceInfo3();    // Device 8301 ~ 8313
            RequestDeviceInfo4();    // Device 8330 ~ 8352
            RequestDeviceInfo5();    // Device 8360 ~ 8383
            RequestDeviceInfo6();    // Device 8392 ~ 8431
            RequestDeviceInfo7();    // Device 1013
            RequestDeviceInfo8();    // Device 1023
            RequestDeviceInfo9();    // Device  1033 ~ 1043
            RequestDeviceInfo10();   // Device 1053
            RequestDeviceInfo11();   // Device 1063
            RequestDeviceInfo12();    // Device 1931
        }

        public bool RequestDeviceInfo1()  // Device 8211 ~ 8263
        {
            Dictionary<List<Device>, int> _deviceList = new Dictionary<List<Device>, int>();

            int stationNum = 0;

            List<Device> _devList_110 = LoadProtocol.GetData((Int32)eStationNum.GY_MECH_110, ref stationNum);
            _deviceList.Add(_devList_110, stationNum);

            // Station Num 112
            stationNum = 0;
            List<Device> _devList_112 = LoadProtocol.GetData((Int32)eStationNum.BCH_AHUZ_112,  ref stationNum);
            _deviceList.Add(_devList_112, stationNum);

            // Station Num 113
            stationNum = 0;
            List<Device> _devList_113 = LoadProtocol.GetData((Int32)eStationNum.IH_AHU_113, ref stationNum);
            _deviceList.Add(_devList_113, stationNum);

            // Station Num 114
            stationNum = 0;
            List<Device> _devList_114 = LoadProtocol.GetData((Int32)eStationNum.KEAS_B1_AHU_114, ref stationNum);
            _deviceList.Add(_devList_114, stationNum);

            // Station Num 115
            stationNum = 0;
            List<Device> _devList_115 = LoadProtocol.GetData((Int32)eStationNum.ICKD_B2_AHU_115, ref stationNum);
            _deviceList.Add(_devList_115, stationNum);

            CreateNextThread(_deviceList, "ThreadInfo_1");
            return true;
        }

        public bool RequestDeviceInfo2()  // Device 8261 ~ 8302
        {
            Dictionary<List<Device>, int> _deviceList2 = new Dictionary<List<Device>, int>();

            //// Station Num 116
            int nStationNum = 0;
            List<Device> _devList_116 = LoadProtocol.GetData((Int32)eStationNum.JJ_AHU_116, ref nStationNum);
            _deviceList2.Add(_devList_116, nStationNum);

            //// Station Num 117
            nStationNum = 0;
            List<Device> _devList_117 = LoadProtocol.GetData((Int32)eStationNum.KALS_AHU_117, ref nStationNum);
            _deviceList2.Add(_devList_117, nStationNum);

            //// Station Num 118
            nStationNum = 0;
            List<Device> _devList_118 = LoadProtocol.GetData((Int32)eStationNum.BPOFF_AHU_118, ref nStationNum);
            _deviceList2.Add(_devList_118, nStationNum);

            //// Station Num 119
            nStationNum = 0;
            List<Device> _devList_119 = LoadProtocol.GetData((Int32)eStationNum.BPMAK_AHU_119, ref nStationNum);
            _deviceList2.Add(_devList_119, nStationNum);
         
            CreateNextThread(_deviceList2, "ThreadInfo_2");
            return true;
        }

        public bool RequestDeviceInfo3()  // Device 8301 ~ 8313
       {
            Dictionary<List<Device>, int> _deviceList3 = new Dictionary<List<Device>, int>();

            //// Station Num 120
            int nStationNum = 0;
            List<Device> _devList_120 = LoadProtocol.GetData((Int32)eStationNum.BP_AHU_120, ref nStationNum);
            _deviceList3.Add(_devList_120, nStationNum);

            //// Station Num 121
            nStationNum = 0;
            List<Device> _devList_121 = LoadProtocol.GetData((Int32)eStationNum.DS_AHU_121, ref nStationNum);
            _deviceList3.Add(_devList_121, nStationNum);

            // Thread Start
            CreateNextThread(_deviceList3, "ThreadInfo_3");

            return true;
        }

        public bool RequestDeviceInfo4()  // Device 8330 ~ 8352
        {
            Dictionary<List<Device>, int> _deviceList4 = new Dictionary<List<Device>, int>();

            // Station Num 122
            int nStationNum = 0;
            List<Device> _devList_122 = LoadProtocol.GetData((Int32)eStationNum.BP3_AHU_122, ref nStationNum);
            _deviceList4.Add(_devList_122, nStationNum);
            
            // Station Num 123
            nStationNum = 0;
            List<Device> _devList_123 = LoadProtocol.GetData((Int32)eStationNum.KANS5_AHU_123, ref nStationNum);
            _deviceList4.Add(_devList_123, nStationNum);

            // Station Num 124
            nStationNum = 0;
            List<Device> _devList_124 = LoadProtocol.GetData((Int32)eStationNum.CITY_AHU_124, ref nStationNum);
            _deviceList4.Add(_devList_124, nStationNum);


            // Thread Start
            CreateNextThread(_deviceList4, "ThreadInfo_4");

            return true;
        }

        public bool RequestDeviceInfo5()  // Device 8360 ~ 8383
        {
            Dictionary<List<Device>, int> _deviceList5 = new Dictionary<List<Device>, int>();

            // Station Num 125
            int nStationNum = 0;
            List<Device> _devList_125 = LoadProtocol.GetData((Int32)eStationNum.ART_AHU_125, ref nStationNum);
            _deviceList5.Add(_devList_125, nStationNum);

            // Station Num 126
            nStationNum = 0;
            List<Device> _devList_126 = LoadProtocol.GetData((Int32)eStationNum.TER_AHU_126, ref nStationNum);
            _deviceList5.Add(_devList_126, nStationNum);

            // Station Num 127
            nStationNum = 0;
            List<Device> _devList_127 = LoadProtocol.GetData((Int32)eStationNum.MH_AHU_127, ref nStationNum);
            _deviceList5.Add(_devList_127, nStationNum);

            // Thread Start
            CreateNextThread(_deviceList5, "ThreadInfo_5");
            
            return true;
        }

        public bool RequestDeviceInfo6()  // Device 8392 ~ 8431
        {
            Dictionary<List<Device>, int> _deviceList6 = new Dictionary<List<Device>, int>();

            // Station Num 128
            int nStationNum = 0;
            List<Device> _devList_128 = LoadProtocol.GetData((Int32)eStationNum.SH_AHU_128, ref nStationNum);
            _deviceList6.Add(_devList_128, nStationNum);

            // Station Num 129
            nStationNum = 0;
            List<Device> _devList_129 = LoadProtocol.GetData((Int32)eStationNum.SYS_129, ref nStationNum);
            _deviceList6.Add(_devList_129, nStationNum);

            // Station Num 130
            nStationNum = 0;
            List<Device> _devList_130 = LoadProtocol.GetData((Int32)eStationNum.WON_AHU_130, ref nStationNum);
            _deviceList6.Add(_devList_130, nStationNum);

            // Station Num 131
            nStationNum = 0;
            List<Device> _devList_131 = LoadProtocol.GetData((Int32)eStationNum.DCH_131, ref nStationNum);
            _deviceList6.Add(_devList_131, nStationNum);

            // Station Num 132
            nStationNum = 0;
            List<Device> _devList_132 = LoadProtocol.GetData((Int32)eStationNum.DM_132, ref nStationNum);
            _deviceList6.Add(_devList_132, nStationNum);


            // Thread Start
            CreateNextThread(_deviceList6, "ThreadInfo_6");

            return true;
        }

        public bool RequestDeviceInfo7()  // Device 1013
        {
            Dictionary<List<Device>, int> _deviceList7 = new Dictionary<List<Device>, int>();

            // Station Num 133
            int nStationNum = 0;
            List<Device> _devList_133 = LoadProtocol.GetData((Int32)eStationNum.COMPASS_OFFICE_133, ref nStationNum);
            _deviceList7.Add(_devList_133, nStationNum);

            // Thread Start
            CreateNextThread(_deviceList7, "ThreadInfo_7");

            return true;
        }

        public bool RequestDeviceInfo8()  // Device 1023
        {
            Dictionary<List<Device>, int> _deviceList8 = new Dictionary<List<Device>, int>();

            // Station Num 134
            int nStationNum = 0;
            List<Device> _devList_134 = LoadProtocol.GetData((Int32)eStationNum.TECHNO_OFFICE_134, ref nStationNum);
            _deviceList8.Add(_devList_134, nStationNum);

            // Thread Start
            CreateNextThread(_deviceList8, "ThreadInfo_8");

            return true;
        }

        public bool RequestDeviceInfo9()  // Device  1033 ~ 1043
        {
           
            Dictionary<List<Device>, int> _deviceList9 = new Dictionary<List<Device>, int>();
            
            // Station Num 135
            int nStationNum = 0;
            List<Device> _devList_135 = LoadProtocol.GetData((Int32)eStationNum.KNOWLEDGE_ZONE_OFFICE_135, ref nStationNum);
            _deviceList9.Add(_devList_135, nStationNum);

            // Station Num 136
            nStationNum = 0;
            List<Device> _devList_136 = LoadProtocol.GetData((Int32)eStationNum.INCHEON_UNIV_136, ref nStationNum);
            _deviceList9.Add(_devList_136, nStationNum);

            // Thread Start
            CreateNextThread(_deviceList9, "ThreadInfo_9");
            
            return true;
        }

        public bool RequestDeviceInfo10()  // Device 1053
        {
            Dictionary<List<Device>, int> _deviceList10 = new Dictionary<List<Device>, int>();

            // Station Num 137
            int nStationNum = 0;
            List<Device> _devList_137 = LoadProtocol.GetData((Int32)eStationNum.CENTRAL_OFFICE_137, ref nStationNum);
            _deviceList10.Add(_devList_137, nStationNum);

            // Thread Start
            CreateNextThread(_deviceList10, "ThreadInfo_10");
            return true;
        }

        public bool RequestDeviceInfo11()  // Device 1063 
        {
            Dictionary<List<Device>, int> _deviceList11 = new Dictionary<List<Device>, int>();

            // Station Num 138
            int nStationNum = 0;
            List<Device> _devList_138 = LoadProtocol.GetData((Int32)eStationNum.INTER_BUSOFFICE_138, ref nStationNum);
            _deviceList11.Add(_devList_138, nStationNum);

            // Thread Start
            CreateNextThread(_deviceList11, "ThreadInfo_11");
            return true;
        }

        public bool RequestDeviceInfo12()  // Device 1931
        {
            Dictionary<List<Device>, int> _deviceList12 = new Dictionary<List<Device>, int>();

            // Station Num 139
            int nStationNum = 0;
            List<Device> _devList_139 = LoadProtocol.GetData((Int32)eStationNum.MOON_LIGHT_OFFICE_139, ref nStationNum);
            _deviceList12.Add(_devList_139, nStationNum);

            // New thread Start
            CreateNextThread(_deviceList12, "ThreadInfo_12");
            return true;
        }

        public void CreateNextThread(Dictionary<List<Device>, int> deviceList, string deviceName)
        {
            SendThread thread = new SendThread(deviceList);
            thread.settinginfo = settinginfo;
            thread.EventSendData += sendBACnetDeviceData.AddQueueData; // AddQueueData;
            thread.Start(deviceName);
            LogWriter.Instance.Write($"{deviceName} device Count: " + deviceList.Count.ToString());
        }

    }

    class BacNode
    {
        BacnetAddress adr;
        uint device_id;

        public BacNode(BacnetAddress adr, uint device_id)
        {
            this.adr = adr;
            this.device_id = device_id;
        }

        public BacnetAddress getAdd(uint device_id)
        {
            if (this.device_id == device_id)
                return adr;
            else
                return null;
        }
    }
    public struct  DeviceInfo
    {
        public string pointName;
        public int pointInstance;
    }
    public struct Device
    {
        public int stationNumber;
        public string ipAddress;
        public int id;
        public DeviceInfo infoList;
        public Device(int stationNumber, string sipAddress, int nid, string pointName, int pointInstance)
        {
            this.stationNumber = stationNumber;
            this.ipAddress = sipAddress;
            this.id = nid;
            this.infoList.pointName = pointName;
            this.infoList.pointInstance = pointInstance;
        }
    }
    class ReadBACnetAddressTable
    {
        private string _path = string.Empty;
        public ReadBACnetAddressTable(string path)
        {
            _path = path;
        }

        // BACnet Protocol List
        public List<Device> BACnetProtocolList = new List<Device>();

        public bool Read()
        {
            string strFile = $"{_path}\\setting\\BACnetProtocolSource.csv";
            if (!System.IO.File.Exists(strFile))return false;

            using (StreamReader file = new StreamReader(strFile))
            {
                string ln;
                
                try
                {
                    while ((ln = file.ReadLine()) != null)
                    {
                        string[] ssplit = ln.Split(",");
                        BACnetProtocolList.Add(new Device(Int32.Parse(ssplit[1]), ssplit[2], Int32.Parse(ssplit[3]), ssplit[4], Int32.Parse(ssplit[5])));
                    }
                }
                catch(System.IO.IOException e)
                {
                    LogWriter.Instance.Write(e.Message);
                    return false;
                }
            }

            return true;
        }

        public List<Device> GetData(int stNum, ref int stationNumber)
        {
            List<Device>  _devList_110 = new List<Device>();
            _devList_110 = Processing.Instance.LoadProtocol.BACnetProtocolList.Where(device => stNum == device.stationNumber).ToList();
            stationNumber = (_devList_110.Count > 0) ? _devList_110[0].stationNumber : 0;

            return _devList_110;
        }
    }
    public class SendBACnetDeviceData
    {
        ConcurrentQueue<List<byte>> queueByteBuffer = new ConcurrentQueue<List<byte>>();
        Thread threadQueueDataSender;
        public SettingInfo settingInfo = new SettingInfo();
        public void StartThread()
        {
            threadQueueDataSender = new Thread(() => StartQueueDataSender());
            threadQueueDataSender.Name = "QueueDataSender";
            threadQueueDataSender.Start();
        }

        Mutex mtx = new Mutex();
        public void AddQueueData(List<byte> list)
        {
            mtx.WaitOne();
            //var newList = list.ConvertAll(new Converter<byte, byte>(x => x));

            //List<byte> newlist = new List<byte>(list.Count);
            // newlist = list.ConvertAll(new Converter<byte, byte>(x => x));
            queueByteBuffer.Enqueue(list);

            // newlist = null;

            mtx.ReleaseMutex();
        }

        public async void StartQueueDataSender()
        {
            while (true)
            {
                if (queueByteBuffer.Count > 0)
                {
                    if (queueByteBuffer.TryDequeue(out List<byte> result))
                    {
                        if (result != null)
                        {
                            if (Processing.Instance._serialPortConnect.isConnected)
                            {
                                byte[] bytes = result.ToArray();

                                await Processing.Instance._serialPortConnect.Write(bytes);
                                Console.WriteLine($"{DateTime.Now.ToString("dd:HH:mm:ss ")}Write Data to Serial Port. Size: {bytes.Length.ToString()}");
                            }
                            else
                            {
                                LogWriter.Instance.Write("Serial Port isn't connected!");
                            }
                        }
                    }
                }
                Thread.Sleep(settingInfo.SendTimeSerialPortData);
            }
        }

    }
}

