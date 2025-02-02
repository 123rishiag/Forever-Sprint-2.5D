using ServiceLocator.Collectible;
using ServiceLocator.Player;
using System.Collections.Generic;
using UnityEngine;

namespace ServiceLocator.Level
{
    public class LevelService
    {
        // Private Variables
        private LevelConfig levelConfig;
        private Transform levelParentPanel;

        private List<LevelController> levelControllers;
        private List<Vector3> nextLevelPositions;

        // Private Services
        private PlayerService playerService;
        private CollectibleService collectibleService;

        public LevelService(LevelConfig _levelConfig, Transform _levelParentPanel)
        {
            // Setting Variables
            levelConfig = _levelConfig;
            levelParentPanel = _levelParentPanel;

            levelControllers = new List<LevelController>();
            nextLevelPositions = new List<Vector3>();
        }

        public void Init(PlayerService _playerService, CollectibleService _collectibleService)
        {
            // Setting Services
            playerService = _playerService;
            collectibleService = _collectibleService;

            // Setting Elements
            StartLevels();
        }

        public void Reset()
        {
            DestroyLevels();
        }

        public void Destroy()
        {
            DestroyLevels();
        }

        public void Update()
        {
            DestroyLevels(true);
            CreateLevels();
        }

        private void StartLevels()
        {
            // Initializing the start position of all Levels
            for (int i = 0; i < levelConfig.levelData.Length; ++i)
            {
                // Adding the calculated position to the list
                Vector3 startPosition = playerService.GetPlayerController().GetTransform().position
                    - Vector3.forward * levelConfig.levelData[i].startPositionOffset;
                nextLevelPositions.Add(startPosition);
            }
        }

        private void DestroyLevels(bool _checkDespawnDistance = false)
        {
            for (int i = levelControllers.Count - 1; i >= 0; i--)
            {
                var levelController = levelControllers[i];

                if (!levelController.IsActive() || !_checkDespawnDistance ||
                    (playerService.GetPlayerController().GetTransform().position.x -
                    levelController.GetTransform().position.x)
                    > levelConfig.deSpawnDistance)
                {
                    levelController.Destroy();
                    levelControllers.Remove(levelController);
                }
            }
        }

        private void CreateLevels()
        {
            // Generating All Level Types
            for (int i = 0; i < levelConfig.levelData.Length; ++i)
            {
                nextLevelPositions[i] = CreateLevel(levelConfig.levelData[i], nextLevelPositions[i]);
            }
        }
        private Vector3 CreateLevel(LevelData _levelData, Vector3 _nextPosition)
        {
            if ((_nextPosition.x - playerService.GetPlayerController().GetTransform().position.x) < levelConfig.spawnDistance &&
                _nextPosition.x <= playerService.GetPlayerController().GetTransform().position.x + levelConfig.spawnDistance)
            {
                // Fetching Random Prefabs, Offset Distance and Height for Level
                LevelView levelPrefab = GetRandomValue(_levelData.levelPrefabs);
                float levelOffsetDistance = GetRandomValue(_levelData.levelOffsetDistances);
                float levelOffsetHeight = GetRandomValue(_levelData.levelOffsetHeights);

                // If the distance between player and new platform position is 
                if ((_nextPosition.x - playerService.GetPlayerController().GetTransform().position.x)
                    < levelConfig.spawnDistance)
                {
                    // Fetching spawn Position based on selected Prefab
                    Vector3 spawnPosition = new Vector3(
                    _nextPosition.x + levelOffsetDistance,
                    levelPrefab.transform.position.y + levelOffsetHeight,
                    levelPrefab.transform.position.z
                    );

                    // Creating Level
                    var levelController = new LevelController(_levelData, levelPrefab,
                        levelParentPanel, spawnPosition,
                        collectibleService);
                    levelControllers.Add(levelController);

                    // Returning End Position of the New Level
                    return levelController.GetEndPointTransform().position;
                }
            }
            return _nextPosition;
        }

        // Getters
        private T GetRandomValue<T>(T[] _list)
        {
            if (_list.Length > 0)
            {
                int randomIndex = Random.Range(0, _list.Length);
                return _list[randomIndex];
            }
            return default;
        }
    }
}