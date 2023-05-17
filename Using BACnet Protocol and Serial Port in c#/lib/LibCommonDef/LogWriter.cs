using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace LibCommonDef
{
    public class LogWriter : SingletonObj<LogWriter>
    {
        public string AppName = "";
        private Queue logs = Queue.Synchronized(queue: new Queue());
        private string m_exePath = string.Empty;
        private string strFile = string.Empty;

        public LogWriter()
        {
            Thread thread = new Thread(() => DoWork());
            thread.IsBackground = true;
            thread.Start();
        }

        private void DoWork()
        {
            while (true)
            {
                while (logs.Count != 0)
                {
                    try
                    {
                        using (StreamWriter w = File.AppendText(strFile))
                            AppendLog((string)logs.Dequeue(), w);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private readonly object _lockObj = new object();
        public void Write(string logMessage)
        {
            lock (_lockObj)
            {
                m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                strFile = m_exePath + "\\" + DateTime.Today.ToString("yyyyMMdd") + $"_{AppName}_log.txt";

                if (!File.Exists(strFile))
                {
                    File.Create(strFile).Dispose();
                }

                logs.Enqueue(logMessage);
            }

            Console.WriteLine(logMessage);
        }

        private void AppendLog(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("[" + DateTime.Now.ToString("HH:mm:ss") + "]");
                txtWriter.Write("  :{0}", logMessage + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
