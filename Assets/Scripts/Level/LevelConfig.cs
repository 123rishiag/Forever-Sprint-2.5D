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
        public LevelView[] levelPrefabs;
        public float[] levelOffsetDistances;
        public float[] levelOffsetHeights;
    }
}