using System;
using LibCommonDef;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Incheon_BACnet
{
    class Program : SingletonObj<Program>
    {
        const int MOD_WIN = 0x008;
        const int VK_WIN = 0x5B;

        [DllImport("user32.dll")]
        static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        static void Main(string[] args)
        {

            LogWriter.Instance.AppName = "Incheon_BACnet";
            LogWriter.Instance.Write("Incheon_BACnet has started.");

            string fileName = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\setting\\setting_BACnet.xml";

            SettingInfo settinginfo = ReadSettingFile<SettingInfo>.Read(fileName);

            if (settinginfo == null)
            {
                LogWriter.Instance.Write("Check the setting_BACnet.xml");
                Console.WriteLine("Check the setting_BACnet.xml");
            }
            else
            {
                RegisterHotKey(IntPtr.Zero, 1, MOD_WIN, VK_WIN);


                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Trace.Listeners.Add(new ConsoleTraceListener());
                Processing.Instance.Run(settinginfo);
                Console.ReadKey();

                UnregisterHotKey(IntPtr.Zero, 1);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = (Exception)e.ExceptionObject;
            LogWriter.Instance.Write("Unhandled Exception: " + exception.Message);
            LogWriter.Instance.Write("Unhandled Object: " + sender.ToString());
            Environment.Exit(1);
        }
    }
    
}
