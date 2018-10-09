using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;

namespace RabbitMQ.Shared.Messaging
{
    public abstract class Service : IDisposable
    {
        protected readonly ConnectionAdapter _connectionAdapter;
        protected readonly Consumer _consumer;

        public Service(ConnectionAdapter connectionAdapter, string exchangeName, string queueName, bool autoAcknowledge)
        {
            _connectionAdapter = connectionAdapter;
            _consumer = new Consumer(_connectionAdapter, exchangeName, GetRoutingKeys(), queueName, autoAcknowledge);
            _consumer.MessageReceivedHandler += OnMessageReceived;
        }

        public Service(ConnectionAdapter connectionAdapter)
        {
            _connectionAdapter = connectionAdapter;
            _consumer = new Consumer(_connectionAdapter);
            _consumer.MessageReceivedHandler += OnMessageReceived;
        }

        void IDisposable.Dispose() => _consumer.Stop();

        public virtual void OnMessageReceived(object sender, BasicDeliverEventArgs ea) => throw new NotImplementedException();
        public virtual IEnumerable<RoutingKey> GetRoutingKeys() => throw new NotImplementedException();
    }
}
