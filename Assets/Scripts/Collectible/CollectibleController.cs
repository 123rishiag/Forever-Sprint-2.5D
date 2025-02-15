using ServiceLocator.Event;
using ServiceLocator.Sound;
using UnityEngine;

namespace ServiceLocator.Collectible
{
    public class CollectibleController
    {
        // Private Variables
        private CollectibleModel collectibleModel;
        private CollectibleView collectibleView;

        // Private Services
        private EventService eventService;

        public CollectibleController(
            CollectibleData _collectibleData, CollectibleView _collectiblePrefab, CollectibleProperty _collectibleProperty,
            Vector3 _spawnPosition, Transform _collectibleParentPanel,
            EventService _eventService)
        {
            // Setting Variables
            collectibleModel = new CollectibleModel(_collectibleData);
            collectibleView = Object.Instantiate(_collectiblePrefab, _collectibleParentPanel).GetComponent<CollectibleView>();
            collectibleView.Init(this);

            // Setting Services
            eventService = _eventService;

            // Setting Elements
            Reset(_collectibleData, _collectibleProperty, _spawnPosition);
        }

        public void Reset(CollectibleData _collectibleData, CollectibleProperty _collectibleProperty, Vector3 _spawnPosition)
        {
            collectibleModel.Reset(_collectibleData);
            collectibleView.SetProperty(_collectibleProperty);
            collectibleView.SetPosition(_spawnPosition);
            collectibleView.ShowView();
        }

        public void AddScore()
        {
            eventService.OnCollectiblePickupEvent.Invoke(collectibleModel.CollectibleScore);
            eventService.PlaySoundEffectEvent.Invoke(SoundType.COLLECTIBLE_PICKUP);
            collectibleView.HideView();
        }

        // Getters
        public bool IsActive() => collectibleView.gameObject.activeInHierarchy;
        public Transform GetTransform() => collectibleView.transform;
        public CollectibleModel GetModel() => collectibleModel;
        public CollectibleView GetView() => collectibleView;
    }
}