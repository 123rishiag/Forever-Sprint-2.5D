using ServiceLocator.UI;

namespace ServiceLocator.Score
{
    public class ScoreService
    {
        // Private Variables
        private int currentScore;

        // Private Services
        private UIService uiService;

        public ScoreService() { }

        public void Init(UIService _uiService)
        {
            // Setting Services
            uiService = _uiService;
        }

        public void Reset()
        {
            // Setting Variables
            currentScore = 0;
            uiService.GetUIController().UpdateScoreText(currentScore);
        }

        // Setters
        public void AddScore(int _score)
        {
            currentScore += _score;
            uiService.GetUIController().UpdateScoreText(currentScore);
        }
    }
}