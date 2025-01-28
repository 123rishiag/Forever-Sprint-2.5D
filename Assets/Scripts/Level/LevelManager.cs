using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] groundPrefabs;
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float spawnDistance;
    [SerializeField] private float deSpawnDistance;

    [SerializeField] private float[] groundSpawnOffsetDistanceRanges;
    [SerializeField] private float[] groundSpawnOffsetHeightRanges;

    [SerializeField] private float[] platformSpawnOffsetDistanceRanges;
    [SerializeField] private float[] platformSpawnOffsetHeightRanges;

    private Vector3 nextGroundPosition;
    private Vector3 nextPlatformPosition;

    private void Start()
    {
        // Initialize the startposition of grounds and platforms
        nextGroundPosition = playerTransform.position - Vector3.forward * 20f;
        nextPlatformPosition = playerTransform.position + Vector3.forward * 30f;
    }

    private void Update()
    {
        // Generating Grounds and Platforms
        nextGroundPosition = GeneratePlatform(groundPrefabs, groundSpawnOffsetDistanceRanges, groundSpawnOffsetHeightRanges,
            nextGroundPosition);
        nextPlatformPosition = GeneratePlatform(platformPrefabs, platformSpawnOffsetDistanceRanges, platformSpawnOffsetHeightRanges,
            nextPlatformPosition);

        // Destroying Grounds and Platforms
        DestroyPlaform();
    }

    private Vector3 GeneratePlatform(GameObject[] _gameObjects, float[] _offsetDistanceRanges, float[] _offsetHeightRanges,
        Vector3 _nextPosition)
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

                // Setting new position
                return newPlatform.transform.Find("EndPoint").position;
            }
        }
        return _nextPosition;
    }

    private void DestroyPlaform()
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
