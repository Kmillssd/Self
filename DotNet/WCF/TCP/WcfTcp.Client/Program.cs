using System;
using WcfTcp.Client.Services;

namespace WcfTcp.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var opsClientService = new OperatorClientService();

            var response = opsClientService.Proxy.SignIn("Kyle", "3232");
            Console.WriteLine("[.] Received '{0}' using proxy directly", response.State);

            response = opsClientService.SignIn("Kyle", "3232");
            Console.WriteLine("[.] Received '{0}' using method wrapper for proxy", response.State);

            Console.ReadKey();
        }
    }
}
