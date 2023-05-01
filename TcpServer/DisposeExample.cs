using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    public class File
    {
        /// <summary>
        /// A File class which acts
        /// as an unmanaged data
        /// </summary>
        public string Name { get; set; }
        public File(string name) { this.Name = name; }
    }
    /// <summary>
    /// File Handler class which
    /// implements IDisposable
    /// interface
    /// </summary>

    public class FileHandler : IDisposable
    {
        private File fileObject = null;

        //managed object
        private static int TotalFiles = 0;

        //boolen varaible to ensure dispose
        // method executes only once
        private bool disposedValue;

        //Costructor

        public FileHandler(string fileName)
        {
            if (fileObject == null)
            {
                TotalFiles++;
                fileObject = new File(fileName);
            }
        }

        //Gets called by below dispose method resource cleaning happens here
        protected virtual void Dispose(bool disposing)
        {
            // check if already disposed
            if (!disposedValue)
            {
                if (disposing)
                {
                    // free managed objects here
                    TotalFiles = 0;
                }

                //free unmanaged objects here
                Console.WriteLine("The {0} has been dispose", fileObject.Name);
                fileObject = null;

                //set the bool value to true
                disposedValue = true;
            }
        }

        //The comsumer object can call
        // the below dispose method
        public void Dispose()
        {
            //Invoke the above virtual 
            // dispose (bool disposing ) method
            Dispose(disposing: true);

            // Notify the garbage collector
            // abour clearing event
            GC.SuppressFinalize(this);
        }

        // Get the details of the file object
        public void GetFileDetails()
        {
            Console.WriteLine("{0} file has been successfully created", fileObject.Name);
        }

        //Destructor should have the following 
        // invocationi in order to dispose 
        //unmanaged objects at the and
        // ~FileHandler() { Dispose(disposing: false); }
    }

}
