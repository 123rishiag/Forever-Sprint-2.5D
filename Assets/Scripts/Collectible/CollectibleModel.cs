using UnityEngine;

namespace ServiceLocator.Collectible
{
    public class CollectibleModel
    {
        public CollectibleModel(CollectibleData _collectibleData)
        {
            CollectibleType = _collectibleData.collectibleType;
            CollectibleTexture = _collectibleData.collectibleTexture;
            CollectibleScore = _collectibleData.collectibleScore;
        }

        // Getters
        public CollectibleType CollectibleType { get; private set; }
        public Texture CollectibleTexture { get; private set; }
        public int CollectibleScore { get; private set; }
    }
}