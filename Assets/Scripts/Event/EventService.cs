using System;

namespace ServiceLocator.Event
{
    public class EventService
    {
        public EventService()
        {
            OnCollectiblePickupEvent = new EventController<Action<int>>();
            UpdateScoreEvent = new EventController<Action<int>>();
        }

        // Event to on Collectible Pickup - Collectible Controller
        public EventController<Action<int>> OnCollectiblePickupEvent { get; private set; }
        public EventController<Action<int>> UpdateScoreEvent { get; private set; }
    }
}