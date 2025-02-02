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
        private List<Vector3> nextLevelPositions;

        // Private Services
        private PlayerService playerService;
        private CollectibleService collectibleService;

        public LevelService(LevelConfig _levelConfig, Transform _levelParentPanel)
        {
            // Setting Variables
            levelConfig = _levelConfig;
            levelParentPanel = _levelParentPanel;
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
            GenerateLevels();
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
            if (levelParentPanel.childCount > 0)
            {
                GameObject levelPrefab = levelParentPanel.GetChild(0).gameObject;

                if (!_checkDespawnDistance ||
                    (playerService.GetPlayerController().GetTransform().position.x - levelPrefab.transform.position.x)
                    > levelConfig.deSpawnDistance)
                {
                    Object.Destroy(levelPrefab);
                }
            }
        }

        private void GenerateLevels()
        {
            // Generating Levels
            for (int i = 0; i < levelConfig.levelData.Length; ++i)
            {
                LevelData levelData = levelConfig.levelData[i];
                nextLevelPositions[i] = GenerateLevel(
                    levelData.levelType, levelData.groundPrefabs,
                    levelData.groundSpawnOffsetDistanceRanges, levelData.groundSpawnOffsetHeightRanges, nextLevelPositions[i]);
            }
        }
        private Vector3 GenerateLevel(LevelType _levelType, GameObject[] _gameObjects,
            float[] _offsetDistanceRanges, float[] _offsetHeightRanges, Vector3 _nextPosition)
        {
            if ((_nextPosition.x - playerService.GetPlayerController().GetTransform().position.x) < levelConfig.spawnDistance &&
                _nextPosition.x <= playerService.GetPlayerController().GetTransform().position.x + levelConfig.spawnDistance)
            {
                // Fetching Offset Distance
                float offsetDistance = GetOffsetValue(_offsetDistanceRanges);
                float offsetHeight = GetOffsetValue(_offsetHeightRanges);

                // Fetching Random Prefab
                GameObject gameObject = GetRandomObjects(_gameObjects);

                // Applying logic
                if ((_nextPosition.x - playerService.GetPlayerController().GetTransform().position.x) < levelConfig.spawnDistance)
                {
                    // Fetching spawn position
                    Vector3 spawnPosition = new Vector3(
                    _nextPosition.x - gameObject.transform.Find("StartPoint").position.x + offsetDistance,
                    gameObject.transform.position.y + offsetHeight,
                    gameObject.transform.position.z
                    );

                    // Instantiating prefab
                    GameObject newPlatform = Object.Instantiate(gameObject, spawnPosition, Quaternion.identity,
                        levelParentPanel);

                    // Creating Collectibles
                    if (_levelType == LevelType.GROUND_TERRAIN || _levelType == LevelType.GROUND_PLATFORM)
                    {
                        collectibleService.GenerateCollectibles(newPlatform.transform);
                    }

                    // Setting new position
                    return newPlatform.transform.Find("EndPoint").position;
                }
            }
            return _nextPosition;
        }

        // Getters
        private float GetOffsetValue(float[] _offsetRanges)
        {
            if (_offsetRanges.Length > 0)
            {
                int rangeValue = Random.Range(0, _offsetRanges.Length);
                return _offsetRanges[rangeValue];
            }
            return 0;
        }
        private GameObject GetRandomObjects(GameObject[] _gameObjects)
        {
            // Randomly selecting prefabs
            int randomValue = Random.Range(0, _gameObjects.Length);
            GameObject gameObject = _gameObjects[randomValue];
            return gameObject;
        }
    }
}