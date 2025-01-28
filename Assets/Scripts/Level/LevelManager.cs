using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float deSpawnDistance;


    private Vector3 nextPartPosition;

    private void Start()
    {
        // Initialize the first plaform positon a little bit ahead of player
        nextPartPosition = playerTransform.position + Vector3.forward * 1f;
    }

    private void Update()
    {
        GeneratePlatform();
        DestroyPlaform();
    }

    private void GeneratePlatform()
    {
        if ((nextPartPosition.x - playerTransform.position.x) < spawnDistance)
        {
            GameObject platformPrefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

            Vector3 spawnPosition = new Vector3(
                nextPartPosition.x - platformPrefab.transform.Find("StartPoint").position.x,
                platformPrefab.transform.position.y,
                platformPrefab.transform.position.z
            );

            GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity, transform);

            nextPartPosition = newPlatform.transform.Find("EndPoint").position;
        }
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
}
