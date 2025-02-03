using ServiceLocator.Player;
using ServiceLocator.Score;
using ServiceLocator.Sound;
using UnityEngine;

namespace ServiceLocator.Collectible
{
    public class CollectibleService
    {
        // Private Variables
        private CollectibleConfig collectibleConfig;
        private Transform collectibleParentPanel;

        private CollectiblePool collectiblePool;

        // Private Services
        private PlayerService playerService;
        private ScoreService scoreService;
        private SoundService soundService;

        public CollectibleService(CollectibleConfig _collectibleConfig, Transform _collectibleParentPanel)
        {
            // Setting Variables
            collectibleConfig = _collectibleConfig;
            collectibleParentPanel = _collectibleParentPanel;

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

            // Setting Elements
            collectiblePool = new CollectiblePool(collectibleConfig, collectibleParentPanel,
                scoreService, soundService);
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
            for (int i = collectiblePool.pooledItems.Count - 1; i >= 0; i--)
            {
                // Skipping if the pooled item's isUsed is false
                if (!collectiblePool.pooledItems[i].isUsed)
                {
                    continue;
                }

                var collectibleController = collectiblePool.pooledItems[i].Item;
                if (!collectibleController.IsActive() ||
                    !_checkDespawnDistance ||
                    (playerService.GetPlayerController().GetTransform().position.x -
                    collectibleController.GetTransform().position.x)
                    > collectibleConfig.deSpawnDistance)
                {
                    ReturnCollectibleToPool(collectibleController);
                }
            }
        }
        public void CreateCollectibles(Bounds _levelBoundary)
        {
            // If Random Probability is not in spawn Probability
            if (Random.value > collectibleConfig.spawnProbability) return;

            // Fetching Collectible Size
            Vector2 collectibleSize = GetCollectibleSize();

            // Fetching Random number of Collectibles to be generated on platform
            int collectibleCount = GetCollectibleCount(_levelBoundary, collectibleSize);

            // Fetching Spacing Between Collectibles based on Collectible Counts and level boundaries
            float collectibleSpacing = GetCollectibleSpacing(_levelBoundary, collectibleSize, collectibleCount);

            // Fetching Positions to spawn collectibles
            float spawnPositionY = _levelBoundary.center.y + collectibleSize.y;
            float startPositionX = _levelBoundary.min.x;

            // Fetching Random Index
            int randomIndex = Random.Range(0, collectibleConfig.collectibleData.Length);
            CollectibleData collectibleData = collectibleConfig.collectibleData[randomIndex];

            // Creating Collectibles
            for (int i = 0; i < collectibleCount; ++i)
            {
                float spawnPositionX = startPositionX + (i * (collectibleSize.x + collectibleSpacing)) + collectibleSpacing +
                    (collectibleSize.x / 2);
                Vector3 spawnPosition = new Vector3(spawnPositionX, spawnPositionY, 0f);

                // Fetching Controller
                CollectibleController collectibleController = null;
                switch (collectibleData.collectibleType)
                {
                    case CollectibleType.CUBE_ONE:
                    case CollectibleType.CUBE_TWO:
                        collectibleController = collectiblePool.GetCollectible<CollectibleController>(
                    collectibleData, collectibleData.collectibleProperty, spawnPosition);
                        break;
                    default:
                        Debug.LogWarning($"Unhandled CollectibleType: {collectibleData.collectibleType}");
                        break;
                }
            }
        }

        private void ReturnCollectibleToPool(CollectibleController _collectibleToReturn)
        {
            _collectibleToReturn.GetView().HideView();
            collectiblePool.ReturnItem(_collectibleToReturn);
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