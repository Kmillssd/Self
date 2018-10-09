using RabbitMQ.Shared.Messaging.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace RabbitMQ.Shared.Messaging
{
    public class RoutingKey
    {
        private readonly string _activity;
        private readonly string _event;

        private static IReadOnlyDictionary<Activity, string> _activities = new Dictionary<Activity, string>()
        {
            { Activity.Wildcard, "*" },
            { Activity.SignOn, "signon" },

        }.ToImmutableDictionary();

        private static IReadOnlyDictionary<Event, string> _events = new Dictionary<Event, string>()
        {
            { Event.Wildcard, "*" },
            { Event.Success, "success" },
            { Event.Failed, "failed" },

        }.ToImmutableDictionary();

        public RoutingKey(Activity activity, Event @event)
        {
            if (!_activities.TryGetValue(activity, out _activity))
            {
                throw new ArgumentException($"activity {nameof(activity)} does not exist");
            }

            if (!_events.TryGetValue(@event, out _event))
            {
                throw new ArgumentException($"activity {nameof(@event)} does not exist");
            }
        }

        public string Create()
        {
            return _activity + '.' + _event;
        }
    }
}
