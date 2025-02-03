using System;
using UnityEngine;

namespace ServiceLocator.Collectible
{
    [CreateAssetMenu(fileName = "CollectibleConfig", menuName = "ScriptableObjects/CollectibleConfig")]

    public class CollectibleConfig : ScriptableObject
    {
        public CollectibleView collectiblePrefab;
        [Space]
        [Range(0, 1)] public float spawnProbability;
        [Range(0, 1)] public float minSpawnRatio;
        [Range(0, 1)] public float maxSpawnRatio;
        public float deSpawnDistance;
        [Space]
        public CollectibleData[] collectibleData;
    }

    [Serializable]
    public class CollectibleData
    {
        public CollectibleType collectibleType;
        public CollectibleProperty collectibleProperty;
        public int collectibleScore;
    }

    [Serializable]
    public class CollectibleProperty
    {
        public Texture collectibleTexture;
    }
}