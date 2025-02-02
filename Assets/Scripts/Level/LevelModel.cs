namespace ServiceLocator.Level
{
    public class LevelModel
    {
        public LevelModel(LevelData _levelData)
        {
            LevelType = _levelData.levelType;
        }

        // Getters
        public LevelType LevelType { get; private set; }
    }
}