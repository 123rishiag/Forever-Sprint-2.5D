using System;
using UnityEngine;

namespace ServiceLocator.Level
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig")]

    public class LevelConfig : ScriptableObject
    {
        public LevelView levelPrefab;
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
        public LevelProperty[] levelProperties;
        public float[] levelOffsetDistances;
        public float[] levelOffsetHeights;
    }

    [Serializable]
    public class LevelProperty
    {
        public Texture levelTexture;
        public Vector3 levelPosition;
        public Quaternion levelRotation;
        public Vector3 levelScale;
    }
}