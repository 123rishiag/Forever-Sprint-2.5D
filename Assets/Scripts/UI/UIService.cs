using ServiceLocator.Event;
using ServiceLocator.Main;

namespace ServiceLocator.UI
{
    public class UIService
    {
        // Private Variables
        private UIController uiController;

        private UIView uiCanvas;
        private GameController gameController;

        public UIService(UIView _uiCanvas, GameController _gameController)
        {
            // Setting Variables
            uiCanvas = _uiCanvas;
            gameController = _gameController;
        }

        public void Init(EventService _eventService)
        {
            // Setting Elements
            uiController = new UIController(uiCanvas, gameController, _eventService);
        }

        public void Destroy() => uiController.Destroy();

        public void Reset() => uiController.Reset();

        // Getters
        public UIController GetUIController() => uiController;
    }
}