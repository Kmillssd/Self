using RabbitMQ.Shared.Messaging;
using RabbitMQ.Server.Services;
using System;

namespace RabbitMQ.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionAdapter = new ConnectionAdapter();

            var operatorService = new OperatorServices(connectionAdapter);

            Console.WriteLine(" [x] Awaiting RPC requests");
            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}





