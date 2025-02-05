using ServiceLocator.Event;
using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Collectible
{
    public class CollectiblePool : GenericObjectPool<CollectibleController>
    {
        // Private Variables
        private CollectibleConfig collectibleConfig;
        private Transform collectibleParentPanel;

        private CollectibleData collectibleData;
        private CollectibleProperty collectibleProperty;
        private Vector2 spawnPosition;


        // Private Services
        private EventService eventService;

        public CollectiblePool(CollectibleConfig _collectibleConfig, Transform _collectibleParentPanel,
            EventService _eventService)
        {
            // Setting Variables
            collectibleConfig = _collectibleConfig;
            collectibleParentPanel = _collectibleParentPanel;

            // Setting Services
            eventService = _eventService;
        }

        public CollectibleController GetCollectible<T>(
            CollectibleData _collectibleData, CollectibleProperty _collectibleProperty, Vector2 _spawnPosition)
            where T : CollectibleController
        {
            // Setting Variables
            collectibleData = _collectibleData;
            collectibleProperty = _collectibleProperty;
            spawnPosition = _spawnPosition;

            // Fetching Item
            var item = GetItem<T>();

            // Resetting Item Properties
            item.Reset(collectibleData, collectibleProperty, spawnPosition);

            return item;
        }

        protected override CollectibleController CreateItem<T>()
        {
            // Creating Controller
            switch (collectibleData.collectibleType)
            {
                case CollectibleType.CUBE_ONE:
                case CollectibleType.CUBE_TWO:
                    return new CollectibleController(
                    collectibleData, collectibleConfig.collectiblePrefab, collectibleProperty,
                    spawnPosition, collectibleParentPanel,
                    eventService);
                default:
                    Debug.LogWarning($"Unhandled CollectibleType: {collectibleData.collectibleType}");
                    return null;
            }
        }
    }
}