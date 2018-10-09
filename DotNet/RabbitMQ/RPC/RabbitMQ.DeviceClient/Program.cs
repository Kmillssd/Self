using RabbitMQ.Shared.Messaging;
using System;

namespace RabbitMQ.DeviceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionAdapter = new ConnectionAdapter();

            var rpcClient = new RpcClientService(connectionAdapter);

            // Check debug console to see responses
            for(var i = 0; i < 10000; i++)
            {
                Console.WriteLine(" [x] Requesting SignOn args KM,3223");

                var response = rpcClient.SignOn("KM", "3223");

                Console.WriteLine(" [.] Got '{0}'", response);
               
            }
         
            Console.ReadKey();
        }
    }
}
