using System;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid guid = Guid.NewGuid();

            FileHandler fileHandler = new FileHandler("Dispose exapmle");
            fileHandler.GetFileDetails();
            //manual calling
            fileHandler.Dispose();

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Implicit calling using 'Using' keyword - 0");
            Console.WriteLine("");

            using (FileHandler fileHandler2 = new FileHandler("Dispose example -2"))
            {
                fileHandler2.GetFileDetails();
            }


            Console.WriteLine("Hello World!" + guid.ToString());
        }
    }
}
