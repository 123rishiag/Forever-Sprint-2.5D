using ServiceLocator.Collectible;
using UnityEngine;

namespace ServiceLocator.Level
{
    public class LevelController
    {
        // Private Variables
        private LevelModel levelModel;
        private LevelView levelView;

        // Private Services
        private CollectibleService collectibleService;

        public LevelController(LevelData _levelData, LevelView _levelPrefab, LevelProperty _levelProperty,
            Vector3 _spawnPosition, Transform _levelParentPanel,
            CollectibleService _collectibleService)
        {
            // Setting Variables
            levelModel = new LevelModel(_levelData);
            levelView = Object.Instantiate(_levelPrefab, _levelParentPanel).GetComponent<LevelView>();

            // Setting Services
            collectibleService = _collectibleService;

            // Setting Elements
            Reset(_levelData, _levelProperty,
            _spawnPosition);
        }

        public void Reset(LevelData _levelData, LevelProperty _levelProperty, Vector2 _spawnPosition)
        {
            levelModel.Reset(_levelData);
            levelView.SetProperty(_levelProperty);
            levelView.SetPosition(_spawnPosition);
            levelView.ShowView();

            // Create Collectibles
            if (levelModel.LevelType == LevelType.GROUND_TERRAIN || levelModel.LevelType == LevelType.GROUND_PLATFORM)
            {
                collectibleService.CreateCollectibles(levelView.GetLevelBounds());
            }
        }

        // Getters
        public bool IsActive() => levelView != null && levelView.gameObject.activeInHierarchy;
        public Transform GetTransform() => levelView.transform;
        public Transform GetEndPointTransform() => levelView.GetEndPointTransform();
        public Bounds GetLevelBounds() => levelView.GetLevelBounds();
        public LevelModel GetModel() => levelModel;
        public LevelView GetView() => levelView;
    }
}