using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    [Header("Level Elements")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float deSpawnDistance;
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] CollectibleManager collectibleManager;

    // Private Variables
    private List<Vector3> nextLevelPositions;

    private void Start()
    {
        // Setting Variables
        nextLevelPositions = new List<Vector3>();
        StartLevels();
    }
    private void StartLevels()
    {
        // Initializing the start position of all Levels
        for (int i = 0; i < levelConfig.levelData.Length; ++i)
        {
            // Adding the calculated position to the list
            Vector3 startPosition = playerTransform.position - Vector3.forward * levelConfig.levelData[i].startPositionOffset;
            nextLevelPositions.Add(startPosition);
        }
    }

    private void Update()
    {
        // Generating Levels
        GenerateLevels();

        // Destroying Levels
        DestroyLevel();
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
        if ((_nextPosition.x - playerTransform.position.x) < spawnDistance)
        {
            // Fetching Offset Distance
            float offsetDistance = GetOffsetValue(_offsetDistanceRanges);
            float offsetHeight = GetOffsetValue(_offsetHeightRanges);

            // Fetching Random Prefab
            GameObject gameObject = GetRandomObjects(_gameObjects);

            // Applying logic
            if ((_nextPosition.x - playerTransform.position.x) < gameObject.transform.localScale.x + offsetDistance)
            {
                // Fetching spawn position
                Vector3 spawnPosition = new Vector3(
                _nextPosition.x - gameObject.transform.Find("StartPoint").position.x + offsetDistance,
                gameObject.transform.position.y + offsetHeight,
                gameObject.transform.position.z
                );

                // Instantiating prefab
                GameObject newPlatform = Instantiate(gameObject, spawnPosition, Quaternion.identity, transform);

                // Creating Collectibles
                if (_levelType == LevelType.Ground_Terrain || _levelType == LevelType.Ground_Platform)
                {
                    collectibleManager.GenerateCollectibles(newPlatform.transform);
                }

                // Setting new position
                return newPlatform.transform.Find("EndPoint").position;
            }
        }
        return _nextPosition;
    }
    private void DestroyLevel()
    {
        if (transform.childCount > 0)
        {
            GameObject platformPrefab = transform.GetChild(0).gameObject;
            if ((playerTransform.position.x - platformPrefab.transform.position.x) > deSpawnDistance)
            {
                Destroy(platformPrefab);
            }
        }
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
