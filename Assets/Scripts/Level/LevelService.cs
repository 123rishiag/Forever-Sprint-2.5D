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

        private LevelPool levelPool;
        private List<Vector3> nextLevelPositions;

        // Private Services
        private PlayerService playerService;
        private CollectibleService collectibleService;

        public LevelService(LevelConfig _levelConfig, Transform _levelParentPanel)
        {
            // Setting Variables
            levelConfig = _levelConfig;
            levelParentPanel = _levelParentPanel;
        }

        public void Init(PlayerService _playerService, CollectibleService _collectibleService)
        {
            // Setting Services
            playerService = _playerService;
            collectibleService = _collectibleService;

            // Setting Elements
            levelPool = new LevelPool(levelConfig, levelParentPanel, collectibleService);
            nextLevelPositions = new List<Vector3>();

            Reset();
        }

        public void Reset()
        {
            nextLevelPositions.Clear();
            DestroyLevels();
            SetLevelsStartPosition();
        }


        public void Update()
        {
            DestroyLevels(true);
            CreateLevels();
        }

        private void SetLevelsStartPosition()
        {
            // Initializing the start position of all Levels
            for (int i = 0; i < levelConfig.levelData.Length; ++i)
            {
                // Adding the calculated position to the list
                float platformStartX = playerService.GetPlayerController().GetTransform().position.x +
                    levelConfig.levelData[i].startPositionOffset;
                Vector3 startPosition = new Vector3(platformStartX, 0, 0);
                nextLevelPositions.Add(startPosition);
            }
        }

        private void DestroyLevels(bool _checkDespawnDistance = false)
        {
            for (int i = levelPool.pooledItems.Count - 1; i >= 0; i--)
            {
                // Skipping if the pooled item's isUsed is false
                if (!levelPool.pooledItems[i].isUsed)
                {
                    continue;
                }

                var levelController = levelPool.pooledItems[i].Item;
                if (!levelController.IsActive() ||
                    !_checkDespawnDistance ||
                    (playerService.GetPlayerController().GetTransform().position.x - levelController.GetTransform().position.x)
                    > levelConfig.deSpawnDistance)
                {
                    ReturnLevelToPool(levelController);
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
                // Fetching Random Property, Offset Distance and Height for Level
                LevelProperty levelProperty = GetRandomValue(_levelData.levelProperties);
                float levelOffsetDistance = GetRandomValue(_levelData.levelOffsetDistances);
                float levelOffsetHeight = GetRandomValue(_levelData.levelOffsetHeights);

                // If the distance between player and new platform position is 
                if ((_nextPosition.x - playerService.GetPlayerController().GetTransform().position.x)
                    < levelConfig.spawnDistance)
                {
                    // Fetching spawn Position based on selected Property
                    Vector3 spawnPosition = new Vector3(
                    _nextPosition.x + levelOffsetDistance,
                    levelProperty.levelPosition.y + levelOffsetHeight,
                    levelProperty.levelPosition.z
                    );

                    // Fetching Level
                    LevelController levelController = null;
                    switch (_levelData.levelType)
                    {
                        case LevelType.BACKGORUND:
                        case LevelType.GROUND_TERRAIN:
                        case LevelType.GROUND_PLATFORM:
                        case LevelType.FOREGROUND:
                            levelController = levelPool.GetLevel<LevelController>(_levelData, levelProperty, spawnPosition);
                            break;
                        default:
                            Debug.LogWarning($"Unhandled LevelType: {_levelData.levelType}");
                            break;
                    }

                    // Returning End Position of the New Level
                    if (levelController != null) return levelController.GetEndPointTransform().position;
                }
            }
            return _nextPosition;
        }

        private void ReturnLevelToPool(LevelController _levelToReturn)
        {
            _levelToReturn.GetView().HideView();
            levelPool.ReturnItem(_levelToReturn);
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