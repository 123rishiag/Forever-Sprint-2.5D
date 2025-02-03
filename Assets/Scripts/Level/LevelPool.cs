using ServiceLocator.Collectible;
using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Level
{
    public class LevelPool : GenericObjectPool<LevelController>
    {
        // Private Variables
        private LevelConfig levelConfig;
        private Transform levelParentPanel;

        private LevelData levelData;
        private LevelProperty levelProperty;
        private Vector2 spawnPosition;


        // Private Services
        private CollectibleService collectibleService;

        public LevelPool(LevelConfig _levelConfig, Transform _levelParentPanel,
            CollectibleService _collectibleService)
        {
            // Setting Variables
            levelConfig = _levelConfig;
            levelParentPanel = _levelParentPanel;

            // Setting Services
            collectibleService = _collectibleService;
        }

        public LevelController GetLevel<T>(LevelData _levelData, LevelProperty _levelProperty, Vector2 _spawnPosition)
            where T : LevelController
        {
            // Setting Variables
            levelData = _levelData;
            levelProperty = _levelProperty;
            spawnPosition = _spawnPosition;

            // Fetching Item
            var item = GetItem<T>();

            // Resetting Item Properties
            item.Reset(levelData, levelProperty, spawnPosition);

            return item;
        }

        protected override LevelController CreateItem<T>()
        {
            // Creating Controller
            switch (levelData.levelType)
            {
                case LevelType.BACKGORUND:
                case LevelType.GROUND_TERRAIN:
                case LevelType.GROUND_PLATFORM:
                case LevelType.FOREGROUND:
                    return new LevelController(levelData, levelConfig.levelPrefab, levelProperty,
                        spawnPosition, levelParentPanel,
                        collectibleService);
                default:
                    Debug.LogWarning($"Unhandled LevelType: {levelData.levelType}");
                    return null;
            }
        }
    }
}