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

        public LevelController(LevelData _levelData, LevelView _levelPrefab,
            Transform _levelParentPanel, Vector3 _spawnPosition,
            CollectibleService _collectibleService)
        {
            // Setting Variables
            levelModel = new LevelModel(_levelData);
            levelView = Object.Instantiate(_levelPrefab, _levelParentPanel).GetComponent<LevelView>();
            levelView.SetPosition(_spawnPosition);

            // Setting Services
            collectibleService = _collectibleService;

            // Setting Elements
            if (levelModel.LevelType == LevelType.GROUND_TERRAIN || levelModel.LevelType == LevelType.GROUND_PLATFORM)
            {
                collectibleService.CreateCollectibles(levelView.GetLevelBounds());
            }
        }

        public void Destroy() => Object.Destroy(levelView.gameObject);

        // Getters
        public bool IsActive() => levelView != null && levelView.gameObject.activeInHierarchy;
        public Transform GetTransform() => levelView.transform;
        public Transform GetEndPointTransform() => levelView.GetEndPointTransform();
        public Bounds GetLevelBounds() => levelView.GetLevelBounds();
        public LevelModel GetModel() => levelModel;
    }
}