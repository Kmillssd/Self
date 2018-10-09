using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ExchangeType = RabbitMQ.Shared.Messaging.Enums.ExchangeType;

namespace RabbitMQ.Shared.Messaging
{
    public static class Exchange
    {
        private static IReadOnlyDictionary<ExchangeType, string> _exchangeTypes = new Dictionary<ExchangeType, string>()
        {
            { ExchangeType.Direct, "direct" },
            { ExchangeType.Topic, "topic" },
            { ExchangeType.FanOut, "fanout" },
            { ExchangeType.Headers, "headers " },

        }.ToImmutableDictionary();

        public static string GetExchangeType(ExchangeType exchangeType)
        {
           
            if (!_exchangeTypes.TryGetValue(exchangeType, out var type))
            {
                throw new ArgumentException($"exchange type {nameof(exchangeType)} does not exist");
            }

            return type;
        }
    }
}
