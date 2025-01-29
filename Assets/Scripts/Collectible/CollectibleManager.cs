using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float deSpawnDistance;
    [SerializeField] private CollectibleConfig collectibleConfig;
    [Space]
    [Range(0, 1)]
    [SerializeField] private float spawnProbability;
    [SerializeField, Range(0, 1)] private float minSpawnRatio;
    [SerializeField, Range(0, 1)] private float maxSpawnRatio;

    private void Start()
    {
        if (maxSpawnRatio < minSpawnRatio)
        {
            maxSpawnRatio = minSpawnRatio;
        }
    }

    private void Update()
    {
        // Destroying Collectibles
        DestroyCollectibles();
    }
    private void DestroyCollectibles()
    {
        if (transform.childCount > 0)
        {
            GameObject collectiblePrefab = transform.GetChild(0).gameObject;
            if ((playerTransform.position.x - collectiblePrefab.transform.position.x) > deSpawnDistance)
            {
                Destroy(collectiblePrefab);
            }
        }
    }

    public void GenerateCollectibles(Transform _spawnTransform)
    {
        if (!ShouldSpawnCollectibles()) return;

        GameObject collectiblePrefab = GetRandomCollectible();
        Vector2 collectibleSize = GetCollectibleSize(collectiblePrefab);
        Bounds spawnBoundary = GetSpawnBounds(_spawnTransform);

        int collectibleCount = GetCollectibleCount(spawnBoundary, collectibleSize);
        float collectibleSpacing = GetCollectibleSpacing(spawnBoundary, collectibleSize, collectibleCount);

        SpawnCollectibles(_spawnTransform, collectiblePrefab, collectibleSize, spawnBoundary, collectibleCount, 
            collectibleSpacing);
    }

    private bool ShouldSpawnCollectibles() => Random.value <= spawnProbability;

    private GameObject GetRandomCollectible()
    {
        int rangeValue = Random.Range(0, collectibleConfig.collectibleData.Length);
        return collectibleConfig.collectibleData[rangeValue].collectiblePrefab;
    }

    private Vector2 GetCollectibleSize(GameObject _collectiblePrefab)
    {
        Renderer collectibleRenderer = _collectiblePrefab.GetComponent<Renderer>();
        return new Vector2(collectibleRenderer.bounds.size.x, collectibleRenderer.bounds.size.y);
    }

    private Bounds GetSpawnBounds(Transform _spawnTransform)
    {
        float topY = _spawnTransform.position.y + (_spawnTransform.localScale.y / 2);
        float leftX = _spawnTransform.position.x - (_spawnTransform.localScale.x / 2);
        float rightX = _spawnTransform.position.x + (_spawnTransform.localScale.x / 2);
        return new Bounds(new Vector3(_spawnTransform.position.x, topY, _spawnTransform.position.z),
                          new Vector3(rightX - leftX, 0, 0));
    }

    private int GetCollectibleCount(Bounds _spawnBoundary, Vector2 _collectibleSize)
    {
        int minCollectibleCount = Mathf.FloorToInt((_spawnBoundary.size.x / _collectibleSize.x) * minSpawnRatio);
        int maxCollectibleCount = Mathf.FloorToInt((_spawnBoundary.size.x / _collectibleSize.x) * maxSpawnRatio);
        return Random.Range(minCollectibleCount, maxCollectibleCount + 1);
    }

    private float GetCollectibleSpacing(Bounds _spawnBoundary, Vector2 _collectibleSize, int _collectibleCount)
    {
        return (_spawnBoundary.size.x - (_collectibleSize.x * _collectibleCount)) / (_collectibleCount + 1);
    }

    private void SpawnCollectibles(Transform _spawnTransform, GameObject _collectiblePrefab, Vector2 _collectibleSize,
                                   Bounds _spawnBoundary, int _collectibleCount, float _collectibleSpacing)
    {
        float spawnPositionY = _spawnBoundary.center.y + _collectibleSize.y;
        float startPostionX = _spawnBoundary.min.x;

        for (int i = 0; i < _collectibleCount; ++i)
        {
            float spawnX = startPostionX + (i * (_collectibleSize.x + _collectibleSpacing)) + _collectibleSpacing + 
                (_collectibleSize.x / 2);
            Vector3 spawnPosition = new Vector3(spawnX, spawnPositionY, _spawnTransform.position.z);

            Instantiate(_collectiblePrefab, spawnPosition, Quaternion.identity, transform);
        }
    }
}