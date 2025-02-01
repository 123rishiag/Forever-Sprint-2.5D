using System;
using UnityEngine;

namespace ServiceLocator.Level
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig")]

    public class LevelConfig : ScriptableObject
    {
        public float spawnDistance;
        public float deSpawnDistance;
        public LevelData[] levelData;
    }

    [Serializable]
    public class LevelData
    {
        public LevelType levelType;
        public float startPositionOffset;
        [Space]
        public GameObject[] groundPrefabs;
        [Space]
        public float[] groundSpawnOffsetDistanceRanges;
        public float[] groundSpawnOffsetHeightRanges;
    }
}