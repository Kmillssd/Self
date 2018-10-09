using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Shared.Messaging
{
    public interface IService
    {
        void OnMessageReceived(object sender, BasicDeliverEventArgs ea);
        IEnumerable<RoutingKey> GetRoutingKeys();
    }
}
