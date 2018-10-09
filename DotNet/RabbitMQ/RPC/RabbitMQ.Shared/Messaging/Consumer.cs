using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using ExchangeType = RabbitMQ.Shared.Messaging.Enums.ExchangeType;

namespace RabbitMQ.Shared.Messaging
{
    public class Consumer : DefaultBasicConsumer
    {
        protected readonly ConnectionAdapter _connectionAdapter;
        protected readonly IEnumerable<RoutingKey> _routingKeys;
        protected readonly bool _autoAcknowledge;
        protected readonly ExchangeType _exchangeType;
        protected readonly ushort _prefetchCount;


        protected readonly string _exchangeName;
        public string ExchangeName
        {
            get
            {
                return _exchangeName;
            }
        }

        protected string _queueName;
        public string QueueName
        {
            get
            {
                return _queueName;
            }
        }

        public event EventHandler<BasicDeliverEventArgs> MessageReceivedHandler;

        public Consumer(ConnectionAdapter connectionAdapter, string exchangeName = null, IEnumerable<RoutingKey> routingKeys = null, string queueName = null, 
            bool autoAcknowledge = true,  bool autoStart = true, ExchangeType exchangeType = ExchangeType.Direct, ushort prefetchCount = 1)
        {
            _connectionAdapter = connectionAdapter;
            _exchangeName = exchangeName;
            _routingKeys = routingKeys;
            _autoAcknowledge = autoAcknowledge;
            _queueName = queueName;
            _exchangeType = exchangeType;
            _prefetchCount = prefetchCount;

            if (autoStart)
                Start();
        }

        public void Start()
        {
            Model = _connectionAdapter.GetConnection().CreateModel();

            if (_queueName != null)
            {
                Model.QueueDeclare(_queueName, true, false, false, null);
            }
            else
            {
                _queueName = Model.QueueDeclare().QueueName;
            }

            if (_exchangeName != null)
            {
                Model.ExchangeDeclare(_exchangeName, Exchange.GetExchangeType(_exchangeType));

                foreach (var routingKey in _routingKeys)
                {
                    Model.QueueBind(_queueName, _exchangeName, routingKey.Create());
                }
            }

            Model.BasicQos(0, _prefetchCount, false);
            Model.BasicConsume(_queueName, _autoAcknowledge, this);
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            MessageReceivedHandler?.Invoke(this, new BasicDeliverEventArgs()
            {
                ConsumerTag = consumerTag,
                DeliveryTag = deliveryTag,
                Redelivered = redelivered,
                Exchange = exchange,
                RoutingKey = routingKey,
                BasicProperties = properties,
                Body = body
            });
        }

        public void Stop()
        {
            if (IsRunning)
            {
                Model.BasicCancel(ConsumerTag);
            }
            Model.Close();
            Model.Dispose();
        }
    }
}

