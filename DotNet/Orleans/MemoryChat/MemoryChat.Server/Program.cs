using System;

namespace MemoryChat.Server
{
    class Program
    {
        static int Main(string[] args)
        {
            var exitCode = 0;
            var serverWrapper = new ServerWrapper();

            exitCode = serverWrapper.Initialise();
            exitCode += serverWrapper.Start();

            Console.WriteLine("Press Enter to terminate...");
            Console.ReadLine();

            exitCode += serverWrapper.Stop();

            return exitCode;
        }
    }
}
