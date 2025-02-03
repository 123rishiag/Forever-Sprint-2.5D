namespace ServiceLocator.Collectible
{
    public class CollectibleModel
    {
        public CollectibleModel(CollectibleData _collectibleData)
        {
            Reset(_collectibleData);
        }

        public void Reset(CollectibleData _collectibleData)
        {
            CollectibleType = _collectibleData.collectibleType;
            CollectibleScore = _collectibleData.collectibleScore;
        }

        // Getters
        public CollectibleType CollectibleType { get; private set; }
        public int CollectibleScore { get; private set; }
    }
}