using ServiceLocator.Event;

namespace ServiceLocator.Score
{
    public class ScoreService
    {
        // Private Variables
        private int currentScore;

        // Private Services
        private EventService eventService;

        public ScoreService() { }

        public void Init(EventService _eventService)
        {
            // Setting Services
            eventService = _eventService;

            // Adding Listeners
            eventService.OnCollectiblePickupEvent.AddListener(AddScore);
        }

        public void Reset()
        {
            // Setting Variables
            currentScore = 0;
        }

        public void Destroy()
        {
            // Removing Listeners
            eventService.OnCollectiblePickupEvent.RemoveListener(AddScore);
        }

        // Setters
        private void AddScore(int _score)
        {
            currentScore += _score;
            eventService.UpdateScoreUIEvent.Invoke(currentScore);
        }
    }
}