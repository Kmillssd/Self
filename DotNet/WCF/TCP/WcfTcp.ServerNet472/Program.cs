using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfTcp.Contracts;
using WctTcp.ServerNet472.Services;

namespace WctTcp.ServerNet472
{
 
    class Program
    {
        static void Main(string[] args)
        {
            var serviceManager = new ServiceManager();

            serviceManager.RegisterService<IOperatorService, OperatorService>("Operators");
            serviceManager.StartServices();

            Console.WriteLine(" [x] Awaiting requests");
            Console.WriteLine("Press [enter] to exit.");
            Console.ReadKey();
        }
    }
}
