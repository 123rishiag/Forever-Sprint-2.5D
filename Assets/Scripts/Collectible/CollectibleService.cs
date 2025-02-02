using ServiceLocator.Player;
using ServiceLocator.Score;
using ServiceLocator.Sound;
using System.Collections.Generic;
using UnityEngine;

namespace ServiceLocator.Collectible
{
    public class CollectibleService
    {
        // Private Variables
        private CollectibleConfig collectibleConfig;
        private Transform collectibleParentPanel;

        private List<CollectibleController> collectibleControllers;

        // Private Services
        private PlayerService playerService;
        private ScoreService scoreService;
        private SoundService soundService;

        public CollectibleService(CollectibleConfig _collectibleConfig, Transform _collectibleParentPanel)
        {
            // Setting Variables
            collectibleConfig = _collectibleConfig;
            collectibleParentPanel = _collectibleParentPanel;

            collectibleControllers = new List<CollectibleController>();

            // Setting Elements
            if (collectibleConfig.maxSpawnRatio < collectibleConfig.minSpawnRatio)
            {
                collectibleConfig.maxSpawnRatio = collectibleConfig.minSpawnRatio;
            }
        }

        public void Init(PlayerService _playerService, ScoreService _scoreService, SoundService _soundService)
        {
            // Setting Services
            playerService = _playerService;
            scoreService = _scoreService;
            soundService = _soundService;
        }

        public void Reset()
        {
            DestroyCollectibles();
        }

        public void Destroy()
        {
            DestroyCollectibles();
        }

        public void Update()
        {
            DestroyCollectibles(true);
        }
        private void DestroyCollectibles(bool _checkDespawnDistance = false)
        {
            for (int i = collectibleControllers.Count - 1; i >= 0; i--)
            {
                var collectibleController = collectibleControllers[i];

                if (!collectibleController.IsActive() || !_checkDespawnDistance ||
                    (playerService.GetPlayerController().GetTransform().position.x -
                    collectibleController.GetTransform().position.x)
                    > collectibleConfig.deSpawnDistance)
                {
                    collectibleController.Destroy();
                    collectibleControllers.Remove(collectibleController);
                }
            }
        }

        public void GenerateCollectibles(Bounds _levelBoundary)
        {
            // If Random Probability is not in spawn Probability
            if (Random.value > collectibleConfig.spawnProbability) return;

            // Fetching Collectible Size
            Vector2 collectibleSize = GetCollectibleSize();

            // Fetching Random number of Collectibles to be generated on platform
            int collectibleCount = GetCollectibleCount(_levelBoundary, collectibleSize);

            // Fetching Spacing Between Collectibles based on Collectible Counts and level boundaries
            float collectibleSpacing = GetCollectibleSpacing(_levelBoundary, collectibleSize, collectibleCount);

            // Creating Collectibles on Plaform
            CreateCollectibles(collectibleSize, _levelBoundary, collectibleCount,
                collectibleSpacing);
        }
        private void CreateCollectibles(Vector2 _collectibleSize, Bounds _levelBoundary, int _collectibleCount,
            float _collectibleSpacing)
        {
            // Fetching Positions to spawn collectibles
            float spawnPositionY = _levelBoundary.center.y + _collectibleSize.y;
            float startPostionX = _levelBoundary.min.x;

            // Fetching Random Index
            int randomIndex = Random.Range(0, collectibleConfig.collectibleData.Length);

            for (int i = 0; i < _collectibleCount; ++i)
            {
                float spawnX = startPostionX + (i * (_collectibleSize.x + _collectibleSpacing)) + _collectibleSpacing +
                    (_collectibleSize.x / 2);
                Vector3 spawnPosition = new Vector3(spawnX, spawnPositionY, 0f);

                var collectibleController = new CollectibleController(
                    collectibleConfig.collectibleData[randomIndex], collectibleConfig.collectiblePrefab,
                    collectibleParentPanel, spawnPosition,
                    scoreService, soundService);

                collectibleControllers.Add(collectibleController);
            }
        }

        // Getters
        private Vector2 GetCollectibleSize()
        {
            Renderer collectibleRenderer = collectibleConfig.collectiblePrefab.GetComponent<Renderer>();
            return new Vector2(collectibleRenderer.bounds.size.x, collectibleRenderer.bounds.size.y);
        }

        private int GetCollectibleCount(Bounds _levelBoundary, Vector2 _collectibleSize)
        {
            int minCollectibleCount = Mathf.FloorToInt((_levelBoundary.size.x / _collectibleSize.x) *
                collectibleConfig.minSpawnRatio);
            int maxCollectibleCount = Mathf.FloorToInt((_levelBoundary.size.x / _collectibleSize.x) *
                collectibleConfig.maxSpawnRatio);
            return Random.Range(minCollectibleCount, maxCollectibleCount + 1);
        }

        private float GetCollectibleSpacing(Bounds _levelBoundary, Vector2 _collectibleSize, int _collectibleCount)
        {
            return (_levelBoundary.size.x - (_collectibleSize.x * _collectibleCount)) / (_collectibleCount + 1);
        }
    }
}