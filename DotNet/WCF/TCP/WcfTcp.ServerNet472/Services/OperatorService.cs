using System;
using System.ServiceModel;
using WcfTcp.Contracts;

namespace WctTcp.ServerNet472.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class OperatorService : IOperatorService
    {
        public string SignIn(string userName, string password)
        {
            Console.WriteLine("Receivied {0} {1} ", userName, password);

            return "Successful";
        }
    }
}


