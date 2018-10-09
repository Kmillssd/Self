using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Shared.Messaging.Enums
{
    public enum ExchangeType
    {
        Direct,
        Topic,
        FanOut,
        Headers
    };
}
