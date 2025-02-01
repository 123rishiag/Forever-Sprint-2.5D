using System;
using UnityEngine;

namespace ServiceLocator.Collectible
{
    [CreateAssetMenu(fileName = "CollectibleConfig", menuName = "ScriptableObjects/CollectibleConfig")]

    public class CollectibleConfig : ScriptableObject
    {
        public CollectibleData[] collectibleData;
    }

    [Serializable]
    public class CollectibleData
    {
        public CollectibleType collectibleType;
        public GameObject collectiblePrefab;
    }
}