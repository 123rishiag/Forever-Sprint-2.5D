using ServiceLocator.Sound;
using System;
using UnityEngine;

namespace ServiceLocator.Event
{
    public class EventService
    {
        public EventService()
        {
            GetPlayerTransformEvent = new EventController<Func<Transform>>();
            CreateCollectiblesEvent = new EventController<Action<Bounds>>();
            OnCollectiblePickupEvent = new EventController<Action<int>>();
            UpdateHealthUIEvent = new EventController<Action<int>>();
            UpdateScoreUIEvent = new EventController<Action<int>>();
            PlaySoundEffectEvent = new EventController<Action<SoundType>>();
        }

        // Event to Get Player Transform - Player Controller
        public EventController<Func<Transform>> GetPlayerTransformEvent { get; private set; }

        // Event to Create Collectibles - Collectible Service
        public EventController<Action<Bounds>> CreateCollectiblesEvent { get; private set; }

        // Event to on Collectible Pickup - Collectible Controller
        public EventController<Action<int>> OnCollectiblePickupEvent { get; private set; }

        // Event to on Update Health UI - UI Controller
        public EventController<Action<int>> UpdateHealthUIEvent { get; private set; }

        // Event to on Update Score UI - UI Controller
        public EventController<Action<int>> UpdateScoreUIEvent { get; private set; }

        // Event to Play Sound Effect - Sound Service
        public EventController<Action<SoundType>> PlaySoundEffectEvent { get; private set; }
    }
}