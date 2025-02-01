using System;
using UnityEngine;

namespace ServiceLocator.Collectible
{
    [CreateAssetMenu(fileName = "CollectibleConfig", menuName = "ScriptableObjects/CollectibleConfig")]

    public class CollectibleConfig : ScriptableObject
    {
        [Range(0, 1)] public float spawnProbability;
        [Range(0, 1)] public float minSpawnRatio;
        [Range(0, 1)] public float maxSpawnRatio;
        public float deSpawnDistance;
        public CollectibleData[] collectibleData;
    }

    [Serializable]
    public class CollectibleData
    {
        public CollectibleType collectibleType;
        public GameObject collectiblePrefab;
    }
}