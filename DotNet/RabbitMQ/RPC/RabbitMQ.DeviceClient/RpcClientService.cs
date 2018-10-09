using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Shared.Messaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.DeviceClient
{
    public class RpcClientService : Service
    {
        protected readonly BlockingCollection<string> _responses;
        protected readonly string _correlationId;
        protected readonly string _replyQueueName;

        public RpcClientService(ConnectionAdapter connectionAdapter) :
            base(connectionAdapter)
        {
            _responses = new BlockingCollection<string>();
            _correlationId = Guid.NewGuid().ToString();
            _replyQueueName = _consumer.QueueName;
        }

        public override void OnMessageReceived(object sender, BasicDeliverEventArgs ea)
        {
            if (ea.BasicProperties.CorrelationId == _correlationId)
            {
                System.Diagnostics.Debug.WriteLine($"({DateTime.Now}) [{this.GetType().FullName}] [HandleBasicDeliver] handling message [{ea.DeliveryTag}] [{ea.Exchange}] [{ea.RoutingKey}] {Encoding.UTF8.GetString(ea.Body)}");

                // Parsing
                _responses.Add(Encoding.UTF8.GetString(ea.Body));
            }
        }
        
        public override IEnumerable<RoutingKey> GetRoutingKeys()
        {
            return new List<RoutingKey>() { };
        }
       
        public string SignOn(string userName, string password)
        {
            var properties = _consumer.Model.CreateBasicProperties();
            properties.CorrelationId = _correlationId;
            properties.ReplyTo = _replyQueueName;

            _consumer.Model.BasicPublish("operator", "signon.*", properties, Encoding.UTF8.GetBytes("KM,32"));

            return _responses.Take();
        }

    }
}
