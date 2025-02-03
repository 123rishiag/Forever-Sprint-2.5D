namespace ServiceLocator.Level
{
    public class LevelModel
    {
        public LevelModel(LevelData _levelData)
        {
            Reset(_levelData);
        }

        public void Reset(LevelData _levelData)
        {
            LevelType = _levelData.levelType;
        }

        // Getters
        public LevelType LevelType { get; private set; }
    }
}