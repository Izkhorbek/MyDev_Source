using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using uint8_t = System.Byte;
using uint16_t = System.UInt16;
using uint32_t = System.UInt32;

using int8_t = System.SByte;
using int16_t = System.Int16;
using int32_t = System.Int32;

namespace LibCommonDef
{
    public enum MsgType
    {
        INFO,
        ERROR,
        WARNING
    }

    public enum PackageId
    {
        NONE = 5,
        RADAR_I_F,
        THREED_VIEW,
        LOG_VIEW,
        SERVER,
        SERVER_REFRESH,
        LANDING_AIRCRAFT_DB,
        LANDING_AIRCRAFT_DRAW,
        PROHIBITED_AREA_DRAW,
        PROHIBITED_AREA_READ,
        PROHIBITED_AREA_READ_FINISHED,
        TRACK_ANALYSIS_DB,
        ARRIEVED_AIRCRAFT,
        FLIGHT_PLAN_INFO,
        ATC_CONTROL,
        ATC_CONTROL_COMMAND,
        ATC_AIRCRAFT_LIST,
        ATC_AIRCRAFT_LIST_SIZE,
        ATC_AIRCRAFT_LIST_STOP,
        ATC_AIRCRAFT_SAVED_DATA,

        RECEIVE_OK = 40
    };

    public enum StandStatus : byte
    {
        /// <summary>
        /// P – Planned
        /// </summary>
        Planned,
        /// <summary>
        /// C – Cancelled
        /// </summary>
        Cancelled,
        /// <summary>
        /// A–  Arrived(by ARTS ATA)
        /// </summary>
        Arrived,
        /// <summary>
        /// D – Departed(by ARTS  ATD)
        /// </summary>
        Departed,
        /// <summary>
        /// O – On block (by AFL On Block time)
        /// </summary>
        OnBlock,
        /// <summary>
        /// F – Off  block (by AFL Off Block time)
        /// </summary>
        OffBlock,
        /// <summary>
        /// Aircraft  on next stand(by AFL On Block time on next stand)
        /// </summary>
        NextStand
    };

    public enum PCOMMAND
    {
        COMMON = 0,
        AircraftState
    }

    public enum CategoryDataType
    {
        ASDE = 1,
        MLAT,
        ADSB,
        ARTS,
        KIMPO,
        WANGSAN
    };

    public enum ATCCONTROL
    {
        NoControl = 0,

        /// <summary>
        /// 항공기 위치 표출
        /// </summary>
        AircraftPosition_AircraftDistance,                                  //항공기 분리거리
        AircraftPosition_GPAngleDistance,                                   //GP거리/각도
        AircraftPosition_RunwayCenterlineDistance,                          //활주로 중심선 거리
        AircraftPosition_VORDMEAngleDistance,                               //VOR/DME 각도거리
        AircraftPosition_AircraftPositionInAirport,                         //선택 항공기 위치
        AircraftPosition_AircraftBearing,                                   //항공기 방위각

        /// <summary>                                                       
        /// 영상 시점 이동                                                   
        /// </summary>                                                      
        VideoPointShift_CameraPosition,                                     //설정시점 이동
        VideoPointShift_SelectAircraftPointOfView,                          //선택 항공기 시점

        /// <summary>                                                       
        /// 녹화 데이터 재생                                                 
        /// </summary>                                                      
        RecordedDataPlayack_ReplayAircraft,                                 //리플레이 항공기
        RecordedDataPlayack_ReplayAllAircraft,                              //전체 리플레이

        /// <summary>
        /// 3D영상 표출 설정
        /// </summary>
        _3D_VideoDisplaySettings_ShowLocalizer,                             //로컬라이저 표출
        _3D_VideoDisplaySettings_ShowGP,                                    //GP 표출
        _3D_VideoDisplaySettings_ShowIM,                                    //IM 표출
        _3D_VideoDisplaySettings_ShowMM,                                    //MM 표출
        _3D_VideoDisplaySettings_ShowObstacleLimitDisplay,                  //장애물제한 표출
        _3D_VideoDisplaySettings_ShowVOR,                                   //VOR 표출
        _3D_VideoDisplaySettings_ShowRadioWaveProtectionArea,               //전파보호구역 표출
        _3D_VideoDisplaySettings_ShowILSandBeaconFacilityOpticalCable,      //ILS/표지시설 관로 표출 
        _3D_VideoDisplaySettings_ShowILSandBeaconFacilityInspectionPoint,   //ILS/표지시설 점검점 표출
        _3D_VideoDisplaySettings_ShowTrackInformation                       //데이터블록 표출
    };

    public enum SWITCH_ONOFF
    {
        Off = 0,
        On = 1
    }

    public enum RUNWAY
    {
        None = 0,
        Runway1,
        Runway2,
        Runway3,
        Runway4,
    }

    public enum RUNWAY_LOCATION
    {
        None,
        _15L,
        _15R,
        _16L,
        _16R,
        _33R,
        _33L,
        _34R,
        _34L
    }

    public enum VOR_DME
    {
        NONE,
        WANGSAN,
        INCHEON
    }

    public enum REPEAT_MODE
    {
        None,
        /// <summary>
        /// 리플레이 항공기
        /// </summary>
        Single,
        /// <summary>
        /// 전체 리플레이
        /// </summary>
        Multiple
    }

    public enum CAMERA_POSITION
    {
        None,
        /// <summary>
        /// 센터
        /// </summary>
        Center,
        /// <summary>
        /// 왼쪽 위
        /// </summary>
        UpperLeft,
        /// <summary>
        /// 오른쪽 위
        /// </summary>
        UpperRight,
        /// <summary>
        /// 왼쪽 하단
        /// </summary>
        BottomLeft,
        /// <summary>
        /// 오른쪽 하단
        /// </summary>
        BottomRight,
    }

    public enum GroupId
    {
        Areas = 1,
        APP_FIXPOINTS = 2,
        AIRPORTS = 3,
        RUNWAYS = 4,
        AIRWAYS = 5,
        LOCALMAP = 6,
        SECTORSBORDER = 7,
        MSAW = 8,
        RWY_IIAC = 9,
        ROU_FIXPOINTS = 10,
        VOR_FIXPOINTS = 11,
        BOUNDARY = 12,
        NTZ = 13,
        STCA_INHIBITION = 14,
        STCA_ZONES = 15
    };

    public enum ComponentType
    {
        Linea = 1,
        Polylinea = 2,
        FixpointRouSymbol = 3,
        FixpointRouText = 4,
        Circumference = 5,
        Circle = 6,
        Text = 7,
        Arc_ = 8,
        AirportSymbol = 9,
        Runway_Line = 10,
        Sector = 11
    };

    public class Constants
    {
        public const double SAKOS_LAT = 37.58955556;  // 15L
        public const double SAKOS_LON = 126.34630556; // 15L
        
        public const double ANBOG_LAT = 37.58738889;  // 15R
        public const double ANBOG_LON = 126.34219444; // 15R

        public const double RUMRI_LAT = 37.53691667;  // 16L
        public const double RUMRI_LON = 126.35844444; // 16L

        public const double OSERI_LAT = 37.53305556;  // 16R
        public const double OSERI_LON = 126.35613889; // 16R


        public const double BATAP_LAT = 37.34852778;  // 33L
        public const double BATAP_LON = 126.55466667; // 33L

        public const double GOKAK_LAT = 37.35066667;  // 33R
        public const double GOKAK_LON = 126.55847222; // 33R

        public const double BODEK_LAT = 37.37702778;  // 34L
        public const double BODEK_LON = 126.49497222; // 34L

        public const double PIPAN_LAT = 37.37919444;  // 34R
        public const double PIPAN_LON = 126.49877778; // 34R
    }

    /// <summary>
    /// Ground Aircraft Movement (GAM)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlightPlaneInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        public byte[] actlOnDate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] arrivalFltInd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] departingFltInd;
        public byte standStat;

        public FlightPlaneInfo(StandStatus status = StandStatus.OffBlock)
        {
            actlOnDate = new UTF8Encoding().GetBytes("????????");
            arrivalFltInd = new UTF8Encoding().GetBytes("????????");
            departingFltInd = new UTF8Encoding().GetBytes("????????");
            standStat = Convert.ToByte(status);
        }

        public void SetArrivalId(string strId)
        {
            byte[] strBytes = Encoding.ASCII.GetBytes(strId);
            for (int i = 0; i < arrivalFltInd.Length; i++)
            {
                if (i < strBytes.Length)
                    arrivalFltInd[i] = strBytes[i];
            }
        }

        public void SetDepartedId(string strId)
        {
            byte[] strBytes = Encoding.ASCII.GetBytes(strId);
            for (int i = 0; i < departingFltInd.Length; i++)
            {
                if (i < strBytes.Length)
                    departingFltInd[i] = strBytes[i];
            }
        }

        public string GetArrivalId()
        {
            return Encoding.Default.GetString(arrivalFltInd);
        }

        public string GetDepartedId()
        {
            return Encoding.Default.GetString(departingFltInd);
        }

        public string GetActualDate()
        {
            return Encoding.Default.GetString(actlOnDate);
        }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LandingAircraftInfo
    {
        public double g_Speed;      // CAT020.RE.GS Ground Speed referenced to WGS-84 ex) 0.0015259 NM/s; NM = 1852 in a second (scale="0.00006103515625" 단위 NM/s  min="0" max="2")
        public double heading;      // CAT020.RE.TA Track Angle clockwise reference to "True North" ex) 227.8619385 deg (scale="0.0054931640625" 단위 deg)
        public double latitude;     
        public double longitude;    
        public double altitude;     // ft min="-1300" max="100000" (scale="25" 단위 ft min="-1300" max="100000" ex)23000.0000000 ft)
        public ushort FL;           // CAT021.145.FL Flight Level ex) 105.7500000 FL
        public short selH;          // CAT021.RE.SelH Selected Heading Status ex) 51.3281250 deg
        public byte am;             // CAT021.RE.AM Approach Mode ,0 Not active, 1: active  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] aircraftID; 
        public ushort categoryType;  // Category type ex) ASDE = 1 ,MLAT = 2, ADSB = 3, ARTS = 4, 김포=5 ,왕산= 6

        public string GetAircraftID()
        {
            return Encoding.Default.GetString(aircraftID);
        }
    }

    public class Table_ILS_Info
    {
        public double Heading { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public ushort FL { get; set; }
        public string AircraftID { get; set; }
        public ushort CategoryType { get; set; }
        public string Runway { get; set; }
        public string SendingTime { get; set; }
    }

    public class ProhibitedAreData
    {
        public int menu_id { get; set; }
        public int group_id { get; set; }
        public int componentType { get; set; }
        public int type_id { get; set; }
        public float n_point1 { get; set; }
        public float e_point1 { get; set; }
        public float n_point2 { get; set; }
        public float e_point2 { get; set; }
        public float n_point3 { get; set; }
        public float e_point3 { get; set; }
        public string name { get; set; }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ProhibitedAreInfo
    { 
        public int menu_id;
        public int group_id;
        public int componentType;
        public int type_id;
        public float n_point1;
        public float e_point1;
        public float n_point2;
        public float e_point2;
        public float n_point3;
        public float e_point3;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HeaderData
    {
        public ushort m_usID;
        public ushort m_usDataSize;
        public ushort m_usEntityArrayIndex;
        public uint m_uiEntityUnique;
        
        public byte m_byBroadCastType;
        public short m_sBlockNumber;
        public uint m_uiSyncUserUnique;

        public HeaderData(PackageId headerId)
        {
            m_usID = (ushort)headerId;
            m_usDataSize = 0;
            m_usEntityArrayIndex = 0;
            m_uiEntityUnique = 0;
            m_sBlockNumber = 0;
            m_byBroadCastType = 0;
            m_uiSyncUserUnique = 0;
        }

        public HeaderData(bool dummy = false)
        {
            m_usID = 0;
            m_usDataSize = 0;
            m_usEntityArrayIndex = 0;
            m_uiEntityUnique = 0;
            m_sBlockNumber = 0;
            m_byBroadCastType = 0;
            m_uiSyncUserUnique = 0;
        } 
    }

    [StructLayout(LayoutKind.Sequential)]
    public class TcpHeaderData
    {
        public byte[] Name;
        public byte[] Method;
        public byte[] Type;
        public int Content_Length;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AircraftData
    {
        public ushort ATCControlCommand;       // ATC Control commands
        public byte ATCControlCommand_On;      // Check if command was on or off 
        public ushort CategoryType;            // Category type ex) ASDE = 1 ,MLAT = 2, ADSB = 3, ARTS = 4, 김포=5 ,왕산= 6
        public byte CategoryNo;                // Category number ex) 020 => 20, 021 = 21, 048 = 48
        public double G_Speed;                 // CAT020.RE.GS Ground Speed referenced to WGS-84 ex) 0.0015259 NM/s; NM = 1852 in a second (scale="0.00006103515625" 단위 NM/s  min="0" max="2")
        public double Heading;                 // CAT020.RE.TA Track Angle clockwise reference to "True North" ex) 227.8619385 deg (scale="0.0054931640625" 단위 deg)
        public double Altitude;                // ft min="-1300" max="100000" (scale="25" 단위 ft min="-1300" max="100000" ex)23000.0000000 ft)
        public double Latitude;
        public double Longitude;
        public double Distance;                // Distance between 2 aircrafts. 단위 NM(Nortical male) for ARTS and ADS-B. For MLAT KM(Kilometer)
        public short SelH;                     // CAT021.RE.SelH Selected Heading Status ex) 51.3281250 deg
        public ushort GVR;                     // CAT021.157.GVR Geometric Vertical Rate ex) 1218.7500000 feet/minute)
        public ushort FL;                      // CAT021.145.FL Flight Level ex) 105.7500000 FL
        public byte APP;                       // CAT020.250.APP  APPROACH Mode ,0 Not active, 1: active    
        public uint ToD;                       // CAT048,CAT020.140.ToD Absolute time stamping expressed as UTC. scale 0.0078125 단위 s  ex)665344 (5198.0000000 s).Time of Day. CAT020.140
        public byte STP;                       // CAT021.RE.STP  (STP value 0 : Aircraft has not stopped 1 : Aircraft has stopped) 
        public double Angle;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] Csn;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] Tyo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] Des;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] Org;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Atd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Actldepdt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Eta;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Exparrdt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Ssrcd;
         
        //public ushort Runway;                  // 활주로 중심선 거리
        //public ushort VorDme;                  // VOR/DME 각도거리
        //public ushort Bearing;                 // 항공기 방위각 
        public ushort CameraPosition;          // 영상 시점 이동

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] Mod3A;                   // CAT020.070.Mod3A  Mode-3/A reply in octal representation ex)2000 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] AircraftID;              // ex) HL7790  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        public byte[] SendingTime;             // Data sending time 

        public AircraftData(bool dump = false)
        {
            ATCControlCommand = (ushort)ATCCONTROL.NoControl;
            ATCControlCommand_On = (byte)SWITCH_ONOFF.Off;
            CategoryType = 2;
            CategoryNo = 20;
            G_Speed = 0.0;
            Heading = 0.0;
            Altitude = 0;
            Latitude = 0.0;
            Longitude = 0.0;
            Distance = 0.0;
            SelH = 0;
            GVR = 0;
            FL = 0;
            APP = 0;
            ToD = 0;
            STP = 0; 
            Angle = 0.0;
             
            CameraPosition = (ushort)CAMERA_POSITION.None;

            Mod3A = new byte[9]; 
            AircraftID = new byte[9]; 
            SendingTime = new byte[19]; 

            Csn = new byte[10];
            Tyo = new byte[5];
            Des = new byte[5];
            Org = new byte[5];
            Atd = new byte[4];
            Actldepdt = new byte[8];
            Eta = new byte[4];
            Exparrdt = new byte[8];
            Ssrcd = new byte[4];
        }

        public void Copy(AircraftData other)
        {
            ATCControlCommand = other.ATCControlCommand;
            ATCControlCommand_On = other.ATCControlCommand_On;
            CategoryType = other.CategoryType;
            CategoryNo = other.CategoryNo;
            G_Speed = other.G_Speed;
            Heading = other.Heading;
            Altitude = other.Altitude;
            Latitude = other.Latitude;
            Longitude = other.Longitude;
            Distance = other.Distance;
            SelH = other.SelH;
            GVR = other.GVR;
            FL = other.FL;
            APP = other.APP;
            ToD = other.ToD;
            STP = other.STP; 
            Angle = other.Angle;
             
            CameraPosition = other.CameraPosition;

            Csn = other.Csn.ToArray();
            Tyo = other.Tyo.ToArray();
            Des = other.Des.ToArray();
            Org = other.Org.ToArray();
            Atd = other.Atd.ToArray();
            Actldepdt = other.Actldepdt.ToArray();
            Eta = other.Eta.ToArray();
            Exparrdt = other.Exparrdt.ToArray();
            Ssrcd = other.Ssrcd.ToArray();

            Mod3A = other.Mod3A.ToArray(); 
            AircraftID = other.AircraftID.ToArray();
            SendingTime = other.SendingTime.ToArray();
        }

        public bool CheckRange()
        {
            if (Latitude > 90 || -90 > Latitude) return false;
            if (Longitude > 180 || -180 > Longitude) return false;

            return true;
        }

        public string GetStrAircraftID()
        {
            return Encoding.Default.GetString(AircraftID);
        }
    };
     
    public struct CategoryData
    {
        public uint16_t CategoryType;    //Category type ex) ASDE = 1 ,MLAT = 2, ADSB = 3, ARTS = 4, 김포=5 ,왕산= 6
        public uint8_t CategoryNo;      //Category number ex) 020 => 20, 021 = 21
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
        public uint8_t[] SendingTime;  //Data sending time  

        /* Data Source Identifier */
        public uint8_t SAC; //CAT48,CAT020.010.SAC SYSTEM AREA CODE    
        public uint8_t SIC; //CAT48,CAT020.010.SIC System Identification Code 


        /*Type and characteristics of the data as transmitted by a system. CATEGORY020.202 */
        public uint8_t SSR; //CAT020.020.SSR     (0: no Non-Mode S 1090MHz multilateration , 1:Non-Mode S 1090MHz multilateration) 
        public uint8_t MS;  //CAT020.020.MS      (0: no Mode-S 1090 MHz multilateration Mode-S , 1: Mode-S 1090 MHz multilateration)        
        public uint8_t HF;  //CAT020.020.HF      (0: no HF multilateration ,1: HF multilateration )                  
        public uint8_t VDL4;//CAT020.020.VDL4    (0: no VDL Mode 4 multilateration ,1:VDL Mode 4 multilateration  )        
        public uint8_t UAT; //CAT020.020.UAT     (0: no UAT multilateration ,1:UAT multilateration )                
        public uint8_t DME; //CAT020.020.DME     (0: no DME/TACAN multilateration ,1:DME/TACAN multilateration  )          
        public uint8_t OT;  //CAT020.020.OT      (0: No Other Technology Multilateration ,1:Other Technology Multilateration  )    
        public uint8_t RAB; //CAT020.020.RAB     (0: Report from target transponder ,1: Report from field monitor (fixed transponder)        
        public uint8_t SPI; //CAT048,CAT020.020.SPI  (0: Absence of SPI ,1:Special Position Identification )                        
        public uint8_t CHN; //CAT020.020.CHN     (0: Chain 1 ,1:Chain2 )                               
        public uint8_t GBS; //CAT020.020.GBS     (0: Transponder Ground bit not set ,1:Transponder Ground bit set )            
        public uint8_t CRT; //CAT020.020.CRT     (0: no Corrupted reply in multilateration ,1:Corrupted reply in multilateration ) 
        public uint8_t SIM; //CAT048,CAT020.020.SIM  (0: Actual target report ,1:Simulated target report )                  
        public uint8_t TST; //CAT020.020.TST     (0: Default ,1:Test Target)                               


        /* Time of Day. CAT020.140 */
        public uint32_t ToD; //CAT048,CAT020.140.ToD Absolute time stamping expressed as UTC. scale 0.0078125 단위 s  ex)665344 (5198.0000000 s)


        /* Position in WGS-84 Coordinates. CAT020.041 */
        public int32_t Lat; // Asterix.CAT020.041 Latitude in WGS-84 in two's complement. 37.4641675 deg -90 ~ 90 scale 아래 참조 
        public int32_t Lon; // Asterix.CAT020.041 Lon Longitude in WGS-84 in two's complement 126.4406794 deg -180 ~ 180 

        /*
        category 20 일때는
        scale 0.000005364418029785156

        category 21 일때는
        scale 0.000021457672119140625

        category 48 일때는
        scale 0.000021457672119140625
        */




        /* Position in Cartesian Coordinates. CAT020.042 */
        public int32_t X;   //Asterix.CAT020.042.X  signed scale 0.5 ex) 134.0000000 m , min="-4194300" max="4194300")
        public int32_t Y;   //Asterix.CAT020.042.Y  signed scale 0.5 ex) 185.0000000 m , min="-4194300" max="4194300")


        /* Track Number. CAT020.161 */
        public uint16_t TrkNb;// Asterix.CAT020.161.TrkNb ex)21 CAT021.161.TrackN 과 같은 항목이어서 TrkNb 로 통합 사용  


        /* Track Status. CAT020.170*/
        public uint8_t CNF; //CAT020.170.CNF (0: Confirmed track , 1: Track in initiation phase)
        public uint8_t TRE; //CAT020.170.TRE (0: Default , 1: Last report for a track<)
        public uint8_t CST; //CAT020.170.CST (0: Not extrapolated, 1:Extrapolated)
        public uint8_t CDM; //CAT020.170.CDM (0: Maintaining, 1: Climbing, 2: Descending, 3: Invalid)
        public uint8_t MAH; //CAT020.170.MAH (0: Default, 1:Horizontal manoeuvre )
        public uint8_t STH; //CAT020.170.STH (0: Measured position , 1: Smoothed position)
        public uint8_t GHO; //CAT020.170.GHO (0: Default ,1: Ghost track)            



        /*Mode-3/A Code in Octal Representation CAT020.070 */
        public uint8_t V; //CAT048.070,CAT020.070.V (0: Code validated, 1: Code validated)                                                 
        public uint8_t G; //CAT048.070,CAT020.070.G (0: Default, 1: Garbled code)
        public uint8_t L; //CAT048.070,CAT020.070.L (0: Mode-3/A code derived from the reply of the transponder, 1:Mode-3/A code not extracted during the last update period)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint8_t[] Mod3A; //CAT048.070 CAT020.070.Mod3A  Mode-3/A reply in octal representation ex)2000


        /*Flight Level in Binary Representation*/
        public uint8_t V_090; //CAT048.090.V ,CAT020.090.V (0: Code validated, 1: Code validated)                                                 
        public uint8_t G_090; //CAT048.090.G ,CAT020.090.G (0: Default, 1: Garbled code)
        public uint16_t FL_090; //CAT048.090.FL, CAT020.090.FL CAT048.090 period scale 0.25 ex) 860 ( 215.0000000 FL ) 





        /*Calculated Track Velocity in Cartesian Coordinates  CAT020.202.*/
        public int16_t Vx; //CAT020.202.Vx ex) (-2.0000000 m/s)  min="-8192" max="8191.75"
        public int16_t Vy; //CAT020.202.Vy ex) (-2.0000000 m/s)  min="-8192" max="8191.75"

        /*Target Address */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint8_t[] TAddr;//CAT020.220.TAddr  Three-octet fixed length Data Item. ex)71BF90

        /*Target Identification */
        public uint8_t STI; // CAT020.245.STI 2 (Callsign downlinked from transponder)아래줄 참조


        /*
        STI 값
        0 : Callsign or registration not downlinked from transponder
        1 : Registration downlinked from transponder
        2 : Callsign downlinked from transponder
        3 : Not defined
        */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint8_t[] TId;//CAT048.240 , Asterix.CAT020.245.TId ex) HL7790  



        /*Selected vertical intention */
        public uint8_t MCP;                // CAT020.250.MCP Altitude Status 1
        public uint16_t MCP_ALT;            // CAT020.250.MCP_ALT MCP/FCU Selected Altitude unsigned.  ft ex) 96.0000000 ft 
        public uint8_t MCP_ALT_STATUS;     // CAT048.250,CAT020.250.MCP_ALT_STATUS  
        public uint8_t FMS_ALT_STATUS;     // CAT048.250,CAT020.250.FMS_ALT_STATUS  FMS Altitude Status 
        public uint16_t FMS_ALT;            // CAT048.250.FMS_ALT,CAT020.250.FMS_ALT FMS Selected Altitude ex)0.0000000 ft                               
        public uint8_t BP_STATUS;          // CAT048.250.BP_STATUS,CAT020.250.BP_STATUS Barometric Pressure Status ex) 0                                           
        public uint16_t BP;                 // CAT048.250.BP CAT020.250.BP  Barometric Pressure ex) (0.0000000 mb)                                   
        public uint8_t MODE_STATUS;        // CAT048.250.MODE_STATUS CAT020.250.MODE_STATUS Status of MCP/FCU Mode Bits ex)0                                         
        public uint8_t VNAV;               // CAT048.250.VNAV, CAT020.250.VNAV  VNAV Mode 0: Not active ,1:active                                   
        public uint8_t ALT_HOLD;           // CAT020.250.ALT_HOLD ALT_HOLD Mode ,0:Not active 1: active                               
        public uint8_t APP;                // CAT020.250.APP  APPROACH Mode ,0 Not active, 1: active                                    
        public uint8_t TARGET_ALT_STATUS;  // CAT020.250.TARGET_ALT_STATUS Status of Target ALT source bits
        /*
        TARGET_ALT_STATUS  value
        0: No source information provided
        1: Source information deliberately provided
        */
        public uint8_t TARGET_ALT_SOURCE;  // CAT020.250.TARGET_ALT_SOURCE Target ALT source아래 값 참조 
        /*
        TARGET_ALT_SOURCE value
        0 : Unknown
        1 : Aircraft Altitude
        2 : FCU/MCP selected altitude
        3 : FMS selected altitude
        */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint8_t[] BDS;             //CAT048.250.BDS,CAT020.250.BDS BDS register ex)40                                                
        //FIXME val is not 8 but need 16 byte
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint8_t[] VAL;             //CAT048.250.VAL CAT020.250.VAL VAL value 
        public uint8_t HDG_STATUS;         //CAT020.250.HDG_STATUS heading status ex)1
        public int16_t HDG;                //CAT020.250.HDG Magnetic Heading
        public uint8_t IAS_STAT_250;           //CAT020.250.ISA_STATUS Indicated Airspeed Status
        public uint16_t IAS_250;                //CAT020.250.ISA Indicated Airspeed 단위 kt 
        public uint8_t MACH_STATUS;           //CAT020.250.MACH_STATUS
        public int16_t MACH;                //CAT020.250.MACH 

        public uint8_t BAR_STATUS;           //CAT020.250.BAR_STATUS Barometric Altitude Rate Status
        public int16_t BAR;           //CAT020.250.BAR Barometric Altitude Rate 단위 ft/min


        public uint8_t IVV_STATUS;           //CAT020.250.IVV_STATUS Inertial Vertical Velocity Status
        public int16_t IVV;           //CAT020.250.BAR Inertial Vertical Velocity 단위 ft/min



        /* Communications/ACAS Capability and Flight Status */
        public uint8_t COM;                // CAT020.230.COM 아래 값 참조 
        /*
        COM value
        0 : No communications capability (surveillance only)
        1 : Comm. A and Comm. B capability
        2 : Comm. A, Comm. B and Uplink ELM
        3 : Comm. A, Comm. B, Uplink ELM and Downlink ELM
        4 : Level 5 Transponder capability
        */

        public uint8_t STAT;// CAT020.230.STAT  아래값 참조 
        /*
        STAT value
        0 : No alert, no SPI, aircraft airborne
        1 : No alert, no SPI, aircraft on ground
        2 : Alert, no SPI, aircraft airborne
        3 : Alert, no SPI, aircraft on ground
        4 : Alert, SPI, aircraft airborne or on ground
        5 : No alert, SPI, aircraft airborne or on ground
        6 : Not assigned
        7 : Information not yet extracted
        */
        public uint8_t MSSC;// CAT020.230.MSSC Mode-S Specific Service Capability 0: No ,1: Yes
        public uint8_t ARC; // CAT020.230.ARC  Altitude reporting capability  0: 100 ft resolution 1: 25 ft resolution
        public uint8_t AIC; // CAT020.230.AIC  Aircraft identification capabilit0  0:No , 1:Yes
        public uint8_t B1A; // CAT020.230.B1A 0
        public uint8_t B1B; // CAT020.230.B1B 0


        /* Reserved Expansion Field of ASTERIX Cat 020 (Multilateration Target Reports) */
        public int16_t SDH;    // CAT020.RE.SDH Standard Deviation of Geometric Height of the target expressed in WGS-84
        public int16_t DOPx;   // CAT020.RE.DOPx DOP along x axis. ex) 1.0000000 
        public int16_t DOPy;   // CAT020.RE.DOPy DOP along y axis. ex) 1.0000000 
        public int16_t DOPxy;  // CAT020.RE.DOPxy p(X,Y). ex) 0.5000000 
        public int16_t SDCx;   // CAT020.RE.SDCx Standard Deviation of Position of the target expressed in Cartesian coordinates (X component) ex) 2.2500000 m
        public int16_t SDCy;   // CAT020.RE.SDCy Standard Deviation of Position of the target expressed in Cartesian coordinates (Y component) ex) 2.2500000 m
        public int16_t COVxy;  // CAT020.RE.COVxy XY Covariance Component in two’s complement form. ex) 1.0000000 m
        public int16_t SDWlat; // CAT020.RE.SDWlat  Standard Deviation of Position of the target expressed in WGS-84 (Latitude component) ex) 0.0000215 deg
        public int16_t SDWlong;// CAT020.RE.SDWlong Standard Deviation of Position of the target expressed in WGS-84 (Longitude component) ex) 0.0000268 deg
        public int16_t COV_WGS;// CAT020.RE.COV_WGS Lat/Long Covariance Component in two’s complement form. ex) 0.0000107 deg
        public uint8_t RE;     // CAT020.RE.RE Range Exceeded Indicator 1:Value in defined range,0:Value exceeds defined range


        // CAT020.RE.GS Ground Speed referenced to WGS-84 ex) 0.0015259 NM/s
        // category20 cateogry21 category48의 공통으로 사용 scale  0.00006103515625 unit NM/s
        public uint16_t GS;
        // category20 cateogry21 category48의 공통으로 사용 scale  0054931640625 unit deg
        public uint16_t TA;     // CAT020.RE.TA Track Angle clockwise reference to "True North" scale="0.0054931640625" unit deg ex) 227.8619385 deg




        public uint8_t GSSD;   // CAT020.RE.GSSD Standard deviation of the Ground Speed ex) 0.0023193 NM/s
        public uint8_t TASD;   // CAT020.RE.TASD Standard deviation of the Track Angle ex)0.0000000 deg
        public int32_t TRT;    // CAT020.RE.TRT Time of ASTERIX Report Transmission ex) 5198.1015625 s
        public uint8_t TI;     // CAT020.RE.TI  Target Identification age ex) 0.0000000 s
        public uint8_t M3A;    // CAT020.RE.M3A  Mode-3/A Code Age ex) 0.0000000 s
        public uint8_t RE_FL;   // CAT020.RE.FL  Flight Level Age  0.0000000 s
        public uint8_t RE_GVR;  // CAT020.295.GVR   Geometric Vertical Rate age

        public uint8_t M2;    // CAT020.RE.M2  Mode-2 Code Age  ex) 0.0000000 s
        public uint8_t M1;    // CAT020.RE.M1  Mode-1 Code Age ex) 0.0000000 s


        /* ######################CAtegory 21 items #############################*/
        public uint8_t ATP;    // CAT021.040.ATP  Address Type. 값 아래 참조
        /*
        ATP value
        0 : 24-Bit ICAO address
        1 : Duplicate address
        2 : Surface vehicle address
        3 : Anonymous address
        */

        public uint8_t RC;     // CAT021.040.RC Range Check. 0: Default , 1: Range Check passed, CPR Validation pending
        public uint8_t DCR;    // CAT021.040.DCR Differential Correction. 
        /*
        DCR value
        0: No differential correction (ADS-B),
        1: Differential correction (ADS-B)
        */

        public uint8_t SAA;    // CAT021.040.SAA Selected Altitude Available, 
        /*
        SAA value
        0:Equipment capable to provide Selected Altitude
        1:Equipment not capable to provide Selected Altitude
        */

        public uint8_t CL;     // CAT021.040.CL Confidence Level
        /*
        CL value
        0: Report valid
        1: Report suspect
        2: No information
        3: Reserved for future use
        */


        /* Track Number TrckNb 와 통합 하여 삭제됨  */
        //public uint16_t            TrackN; // CAT021.161.TrackN  Track Number ex) 3035


        /* Service Identification */
        public uint8_t id;// CAT021.015.id Service Identification

        /* Time of Message Reception for Position */
        public uint32_t time_reception_position;// CAT021.073.time_reception_position. time_reception_position ex) 5197.1796875 s


        /* Time of Message Reception for Velocity */
        /* Time of reception of the latest velocity squitter in the Ground Station, in the form of elapsed time since last midnight, expressed as UTC */
        public uint32_t time_reception_velocity;// CAT021.075.time_reception_velocity. ex)5197.1796875 s


        /* Quality Indicators */
        /* ADS-B quality indicators transmitted by a/c according to MOPS version. */

        /* Navigation Uncertainty Category for velocity or Navigation Accuracy Category for Velocity*/
        public uint8_t NUCr_or_NACv;   // CAT021.090.NUCr_or_NACv ex) 0
        /* Navigation Uncertainty Category for Position NUCp or Navigation Integrity Category NIC.*/
        public uint8_t NUCp_or_NIC;    // CAT021.090.NUCp_or_NIC ex)8
        /* Navigation Integrity Category for Barometric Altitude */
        public uint8_t NICbaro;        // CAT021.090.NICbaro ex)0
        /* Surveillance (version 1) or Source (version 2) Integrity Level*/
        public uint8_t SIL;            // CAT021.090.SIL ex)0
        /* Navigation Accuracy Category for Position */
        public uint8_t NACp;           // CAT021.090.NACp ex)0
        public uint8_t SILS;           // CAT021.090.SILS 0 (Measured per flight-hour) 0 : Measured per flight-hour,1 : Measured per sample
        /* Horizontal Position System Design Assurance Level (as defined in version 2)*/
        public uint8_t SDA;            // CAT021.090.SDA ex)0
        /* Geometric Altitude Accuracy */
        public uint8_t GVA;            // CAT021.090.GVA ex)0
        /* Position Integrity Category */
        public uint8_t PIC;            // CAT021.090.PIC 13
        public uint8_t VNS;// CAT021.210.VNS  Version Not Supported  아래값 참조
        /*
        VNS value
        0 : The MOPS Version is supported by the GS
        1 : The MOPS Version is not supported by the GS
        */

        public uint8_t VN;// CAT021.210.VN Version Number  아래값 참조
        /*
        VN value
        0 : DO-260 [Ref. 8]
        1 : DO-260A [Ref. 9]
        2 : DO-260B
        */

        public uint8_t LTT;// CAT021.210.LTT Link Technology Type  아래값 참조
        /*
        LTT value
        0 : Other
        1 : UAT
        2 : 1090 ES
        3 : VDL 4
        */

        /* Target Status 200 */
        public uint8_t ICF; // CAT021.200.ICF Intent Change Flag
        /*
        ICF value
        0 : No intent change active
        1 : Intent change flag raised
        */

        public uint8_t LNAV;// CAT021.200.LNAV LNAV Mode
        /*
        LNAV value
        0 : LNAV Mode engaged
        1 : LNAV Mode not engaged
        */

        public uint8_t ME;  // 0CAT021.200.ME Military emergency
        /*
        ME value
        0 : No military emergency
        1 : Military emergency
        */

        public uint8_t PS;  // 0CAT021.200.PS Priority Status
        /*
        PS value
        0 : No emergency / not reported
        1 : General emergency
        2 : Lifeguard / medical emergency
        3 : Minimum fuel
        4 : No communications
        5 : Unlawful interference
        6 : Downed Aircraft
        */

        public uint8_t SS; //CAT021.200.SS Surveillance Status
        /*
        SS value
        0 : No condition reported
        1 : Permanent Alert (Emergency condition)
        2 : Temporary Alert (change in Mode 3/A Code other than emergency)
        3 : SPI set
        */

        /* Time of the transmission of the ASTERIX category 021 report in the form of elapsed time since last midnight, expressed as UTC */
        public uint32_t time_report_transmission; //CAT021.077.time_report_transmission ex)  0.0000000 s



        /* Emitter Category */
        /* Characteristics of the originating ADS-B unit. */
        public uint8_t ECAT;// CAT021.020.ECAT  Emitter Category  아래값 참조
        /*
        ECAT value
        0 : No ADS-B Emitter Category Information</BitsValue>
        1 : light aircraft <= 15500 lbs
        2 : 15500 lbs &lt; small aircraft < 75000 lbs
        3 : 75000 lbs &lt; medium a/c < 300000 lbs
        4 : High Vortex Large
        5 : 300000 lbs <= heavy aircraft
        6 : highly manoeuvrable (5g acceleration capability) and high speed (> 400 knots cruise)
        7 : reserved
        8 : reserved
        9 : reserved
        10 : rotocraf
        11 : glider / sailplane
        12 : lighter-than-air
        13 : unmanned aerial vehicle
        14 : space / transatmospheric vehicle
        15 : ultralight / handglider / paraglider
        16 : parachutist / skydiver
        17 : reserved
        18 : reserved
        19 : reserved
        20 : surface emergency vehicle
        21 : surface service vehicle
        22 : fixed ground or tethered obstruction
        23 : cluster obstacle
        24 : line obstacle
        */


        /* Service Management */
        /* Identification of services offered by a ground station (identified by a SIC code). */
        public uint8_t RP; //Asterix.CAT021.016.RP Report Period ex)0 (0.0000000 s)


        public uint8_t POA;// CAT021.271.POA Position Offset Applied
        /*
        POA value
        0 : Position transmitted is not ADS-B position reference point
        1 : Position transmitted is the ADS-B position reference point
        */
        public uint8_t CDTI_S; // CAT021.271.CDTI_S Cockpit Display of Traffic Information Surface
        /*
        CDTI_S value
        0 : CDTI not operational
        1 : CDTI operational
        */
        public uint8_t B2_low; // CAT021.271.B2_low Class B2 transmit power less than 70 Watts
        /*
        B2_low value
        0 : >= 70 Watts
        1 : < 70 Watts
        */

        public uint8_t RAS;    // CAT021.271.RAS 0 Receiving ATC Services
        /*
        RAS value
        0 : Aircraft not receiving ATC-services
        1 : Aircraft receiving ATC services
        */
        public uint8_t IDENT;  // CAT021.271.IDENT Setting of IDENT-switch
        /*
        IDENT value
        0 : IDENT switch not active
        1 : IDENT switch active
        */

        /* Receiver ID */
        /* Designator of Ground Station in Distributed System */
        public uint8_t RID; // CAT021.400.RID Receiver ID
        public uint8_t TRD; // CAT021.295.TRD Age of the Target Report Descriptor, item I021/040 
        public uint8_t QI;  // CAT021.295.QI Age of the Quality Indicators, item I021/090 ex) 
        public uint8_t TID; // CAT021.295.TID Age of the Target Identification, item I021/170
        public uint8_t TS_295; // CAT021.295.TS Age of the Target Status as contained, item I021/200


        /* Reserved Expansion Field */
        /* Reserved Expansion Field of ASTERIX Cat 021 (ADS-B Reports), Edition 1.4 */
        public uint8_t GAO; //CAT021.RE.GAO GPS Antenna Offset
        public uint8_t STP; //CAT021.RE.STP 
        /*
        STP value
        0 : Aircraft has not stopped
        1 : Aircraft has stopped
        */

        public uint8_t HTS; //CAT021.RE.HTS Heading/Ground Track
        /*
        HTS value
        0 : Heading/Ground Track is not valid
        1 : Heading/Ground Track is valid
        */

        public uint8_t HTT; //CAT021.RE.HTT Heading/Ground data
        /*
        HTT value
        0 : Heading data provided
        1 : Ground Track provided
        */

        public uint8_t HRD; //CAT021.RE.HRD SGV Horizontal Reference Direction
        /*
        HRD value
        0 : True North
        1 : Magnetic North
        */
        public uint8_t GSS;//CAT021.RE.GSS Ground Speed ex)0.0000000 kt
        public uint8_t HGT;//CAT021.RE.HGT Heading/Ground Track information ex)0.0000000 deg


        /* Time of Message Reception of Velocity-High Precision */
        /* Time at which the latest ADS-B velocity information was received by the ground station, expressed as fraction of the second of the UTC Time. */
        public uint8_t FSI;// CAT021.076.FSI    Time of Message Reception of Position-High Precision
        public uint8_t FSI_074;// CAT021.074.FSI  Time of Message Reception of Velocity-High Precision
        /*
        FSI value
        0 : TOMRv whole seconds = (I021/073) Whole seconds
        1 : TOMRv whole seconds = (I021/073) Whole seconds + 1
        2 : TOMRv whole seconds = (I021/073) Whole seconds - 1
        */

        public uint32_t time_reception_position_highprecision; // CAT021.074.time_reception_position_highprecision
        public uint32_t time_reception_velocity_highprecision; // CAT021.076.time_reception_velocity_highprecision



        /* Minimum height from a plane tangent to the earth's ellipsoid, defined by WGS-84, in two's complement form.*/
        public int32_t geometric_height;//CAT021.140.geometric_height ex) 10450.0000000 ft

        /* Mode 3/A Code in Octal Representation  CAT020.070.Mod3A  와 같은 항목이어서 삭제됨*/
        //public uint8_t             Mode3A[8]; // CAT021.070.Mode3A Mode-3/A reply in octal ex) 4333



        /* Flight Level */
        public uint16_t FL; //CAT021.145.FL  ex) 105.7500000 FL

        /* Geometric Vertical Rate */
        public uint16_t GVR;// CAT021.157.GVR  ex)1218.7500000 feet/minute)


        public uint8_t SAS;//CAT021.146.SAS
        /*
        SAS value
        0 : No source information provided
        1 : Source Information provided
        */

        public uint8_t Source;//CAT021.146.Source
        /*
        Source value
        0 : Unknown
        1 : Aircraft Altitude (Holding Altitude)
        2 : FCU/MCP Selected Altitude
        3 : FMS Selected Altitude
        */
        public int32_t Alt; // CAT021.146.Alt  alitude ex)23000.0000000 ft scale 25 signed min="-1300" max="100000"

        /* Aircraft Operational Status */
        public uint8_t RA;         // CAT021.008.RA TCAS Resolution Advisory active
        /*
        RA value
        0 : TCAS II or ACAS RA not active
        1 : TCAS RA active
        */
        public uint8_t TC;         // CAT021.008.TC Target Trajectory Change Report Capability
        /*
        TC value
        0 : no capability for Trajectory Change Reports
        1 : support for TC+0 reports only
        2 : support for multiple TC reports
        */
        public uint8_t TS;         // CAT021.008.TS Target State Report Capability
        /*
        TS value
        0 : no capability to support Target State Reports</BitsValue>
        1 : capable of supporting target State Reports</BitsValue>
        */
        public uint8_t ARV;        // CAT021.008.ARV Air-Referenced Velocity Report Capability
        /*
        ARV value
        0 : no capability to generate ARV-reports
        1 : capable of generate ARV-reports
        */
        public uint8_t CDTI_A;     // CAT021.008.CDTI_A Cockpit Display of Traffic Information airborne
        /*
        CDTI_A value
        0 : CDTI not operational
        1 : CDTI operational
        */
        public uint8_t not_TCAS;   // CAT021.008.not_TCAS TCAS System Status
        /*
        not_TCAS value
        0 : TCAS operational or unknown
        1 : TCAS not installed or not operational
        */
        public uint8_t SA;         // CAT021.008.SA Single Antenna
        /*
        SA value
        0 : Antenna Diversity
        1 : Single Antenna only
        */


        /* Message Amplitude */
        /* Amplitude, in dBm, of ADS-B messages received by the ground station */
        public int16_t MAM;//CAT021.132.MAM Message Amplitude ex)-62.0000000 dBm
        public uint8_t MAM_295;//CAT021.295.MAM Message Amplitude age 


        /* Data Ages */
        public uint8_t AOS; // CAT021.295.AOS  Aircraft Operational Status age ex) 1.2000000 s
        public uint8_t GH;  //  CAT021.295.GH  Geometric Height age ex) 0.0000000 s
        public uint8_t ISA; // CAT021.295.ISA Intermediate State Selected Altitude age ex) 1.2000000 s
        public uint8_t BVR; // CAT021.295.BVR  Barometric Vertical Rate age ex) 0.0000000 s
        public int16_t BVR_155; // CAT021.155.BVR_155  Barometric Vertical Rate
        public uint8_t GV; // CAT021.295.GV  Ground Vector age ex) 1.2000000 s


        public uint16_t BPS;        // CAT021.RE.BPS  Barometric Pressure Setting ex) 222.4000000 hPa 
        public uint8_t SelHHRD;    // CAT021.RE.SelHHRD SelH Horizontal Reference Direction 
        /*
        SelHHRD value
        0 : True North
        1 : Magnetic North
        */
        public uint8_t Stat;       // CAT021.RE.Stat
        /*
        Stat value
        Data is either unavailable or invalid
        Data is available and valid
        */

        public int16_t SelH;       // CAT021.RE.SelH Selected Heading Status ex) 51.3281250 deg
        public uint8_t AP;         // CAT021.RE.AP Autopilot 
        /*
        AP value
        0 : Autopilot not engaged
        1 : Autopilot engaged
        */
        public uint8_t RE_VN;      // CAT021.RE.VN  VNAV (Vertical Navigation) 중복되는 VN이있어 RE_VN 으로변경

        /*
        RE_VN value
        0 : VNAV not active
        1 : VNAV active
        */
        public uint8_t AH;         //CAT021.RE.AH  Altitude Hold
        /*
        AH value
        0 : Altitude Hold not engaged
        1 : Altitude Hold engaged
        */
        public uint8_t AM;         // CAT021.RE.AM Approach Mode
        /*
        AM value
        0 : Approach Mode not active
        1 : Approach Mode active
        */
        public uint8_t ES;         // CAT021.RE.ES 1090 ES IN capability 
        /*
        ES value
        0 : arget is not 1090 ES IN capable
        1 : arget is 1090 ES IN capable
        */
        public uint8_t RE_UAT;    // CAT021.RE.UAT  동일한 변수명이 있어 RE_ 를 붙임
        /*
        RE_UAT value
        0 : Target is not UAT IN capable
        1 : Target is UAT IN capable
        */


        /*============================================================================== 
          ===============================category 48 에 있는 항목======================== 
          ==============================================================================*/


        public uint8_t TYP;    // CAT048.020.TYP Target Report Descriptor. 값 아래 참조
        /*
        0 : No detection
        1 : Single PSR detection
        2 : Single SSR detection
        3 : PSR+SSR detection
        4 : Single ModeS All-Call
        5 : Single ModeS Roll-Call
        6 : PSR+ModeS All-Call
        7 : PSR+ModeS Roll-Call
        */


        //CAT 65 번일경우 다음 값을 가짐 
        /*
        1 : SDPS Status
        2 : End of Batch
        3 : Service Status Report
        */


        public uint8_t RDP;         // CAT048.008.RDP 값 아래 참조
        /*
        0 : Report from RDP Chain 1
        1 : Report from RDP Chain 2
        */




        public uint16_t RHO;  //Asterix.CAT048.040.RHO Measured Position in Polar Coordinates max 256 scale =0.00390625 단위 NM
        public uint16_t THETA;//Asterix.CAT048.040.THETA Measured Position in Polar Coordinates scale = 0.0054931640625 단위 deg

        /* Calculated position of an aircraft in cartesian coordinates CAT048.042 */
        public int16_t X_48;   //Asterix.CAT048.042.X  signed scale = 0078125 단위 NM
        public int16_t Y_48;   //Asterix.CAT048.042.Y  signed scale = 0078125  단위 NM


        public uint8_t SRL;         //Asterix.CAT048.130.SRL SSR Plot Runlength scale 0.0439453125 단위 deg 
        public uint8_t SRR;         //Asterix.CAT048.130.SRR SSR-Number of received reply 
        public int8_t SAM;         //Asterix.CAT048.130.SAM SSR-Reply amplitude signed value scale 1
        public uint8_t PRL;         //Asterix.CAT048.130.PRL Primary Plot Runlength scale="0.0439453125" 단위 deg
        public int8_t PAM;         //Asterix.CAT048.130.PAM Amplitude of Primary plot signed scale 1 단위 dBm
        public int8_t RPD;         //Asterix.CAT048.130.RPD Difference in Range between PSR and SSR plot scale="0.00390625" 단위NM
        public int8_t APD;         //Asterix.CAT048.130.APD Difference in Azimuth between PSR and SSR plot scale="0.02197265625" 단위 deg


        /*Aircraft Address */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint8_t[] ACAddr;//CAT048.220.ACAddr  71C086 


        public uint8_t RA_STATUS; //CAT048.250.RA_STATUS Roll Angle Statu

        public int16_t RA_250; // Asterix.CAT048.250.RA roll angle  signed scale="0.17578125"" 단위 deg

        public uint8_t TTA_STATUS; //CAT048.250.TTA_STATUS True Track Angle Status
        public int16_t TTA; // Asterix.CAT048.250.TTA  True Track Angle  signed scale="0.17578125"" 단위 deg

        public uint8_t GS_STATUS; //CAT048.250.GS_STATUS  Ground Speed Status
        public uint16_t GS_250; //CAT048.250.GS scale="2" 단위kt 


        public uint8_t TAR_STATUS; //CAT048.250.TAR_STATUS Track Angle Rate Status
        public int16_t TAR; //CAT048.250.TAR Track Angle Rate scale="0.03125" 단위 deg/sec

        public uint8_t TAS_STATUS; //CAT048.250.TAS_STATUS  True Airspeed Stat
        public int16_t TAS; //CAT048.250.TAS Airspeed scale="2" 단위kt 


        public uint8_t D; // CAT048.120.D Doppler Speed Validation 아래값참조
        /*
        0 : Doppler speed is valid
        1 : Doppler speed is doubtful
        */


        public int16_t CAL; //CAT048.120.CAL Calculated Doppler Speed scale 1 단위 m/sec



        public uint8_t SI; //CAT048.230.SI SI/II Transponder Capability 아래 값 참조
        /*
        0 : SI-Code Capable
        1 : II-Code Capable
        */


        public uint8_t ModeSSSC; //CAT048.230.ModeSSSC ModeS Specific Service Capability   아래 값 참조
        /*
            0 : No
            1 : Yes
        */



        public uint8_t BDS16; //CAT048.230.BDS16 BDS 1,0 bit 16
        public uint8_t BDS37; //CAT048.230.BDS37 BDS 1,0 bits 37/40


        public uint32_t pressure; // scale 0.01 
    };
}
