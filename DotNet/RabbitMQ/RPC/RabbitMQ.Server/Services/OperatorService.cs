using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Shared.Messaging;
using RabbitMQ.Shared.Messaging.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Server.Services
{
    public class OperatorServices : Service
    {
        public OperatorServices(ConnectionAdapter connectionAdapter) : 
            base(connectionAdapter, "operator", "screen_to_voice_queue", false)
        { }

        public override void OnMessageReceived(object sender, BasicDeliverEventArgs ea)
        {
            System.Diagnostics.Debug.WriteLine($"({DateTime.Now}) [{this.GetType().FullName}] [HandleBasicDeliver] handling message [{ea.DeliveryTag}] [{ea.Exchange}] [{ea.RoutingKey}] {Encoding.UTF8.GetString(ea.Body)}");

            string response = null;

            // Ensure the ability to respond
            var replyProps = _consumer.Model.CreateBasicProperties();
            replyProps.CorrelationId = ea.BasicProperties.CorrelationId;

            response = "Successful";

            _consumer.Model.BasicPublish("", ea.BasicProperties.ReplyTo, replyProps, Encoding.UTF8.GetBytes(response));
            _consumer.Model.BasicAck(ea.DeliveryTag, false);
        }

        public override IEnumerable<RoutingKey> GetRoutingKeys()
        {
            return new List<RoutingKey>()
            {
                new RoutingKey(Activity.SignOn, Event.Wildcard),
            };
        }
    }
}
