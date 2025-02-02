using ServiceLocator.Score;
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
        private ScoreService scoreService;
        private SoundService soundService;

        public CollectibleController(CollectibleData _collectibleData, CollectibleView _collectiblePrefab,
            Transform _collectibleParentPanel, Vector3 _spawnPosition,
            ScoreService _scoreService, SoundService _soundService)
        {
            // Setting Variables
            collectibleModel = new CollectibleModel(_collectibleData);
            collectibleView = Object.Instantiate(_collectiblePrefab, _collectibleParentPanel).GetComponent<CollectibleView>();
            collectibleView.Init(this);
            collectibleView.SetPosition(_spawnPosition);

            // Setting Services
            scoreService = _scoreService;
            soundService = _soundService;
        }

        public void Destroy() => Object.Destroy(collectibleView.gameObject);

        public void AddScore()
        {
            scoreService.AddScore(collectibleModel.CollectibleScore);
            collectibleView.HideView();
            soundService.PlaySoundEffect(SoundType.COLLECTIBLE_PICKUP);
        }

        // Getters
        public bool IsActive() => collectibleView != null && collectibleView.gameObject.activeInHierarchy;
        public Transform GetTransform() => collectibleView.transform;
        public CollectibleModel GetModel() => collectibleModel;
    }
}