using ServiceLocator.Event;
using ServiceLocator.Main;

namespace ServiceLocator.UI
{
    public class UIService
    {
        // Private Variables
        private UIView uiCanvas;
        private UIController uiController;

        public UIService(UIView _uiCanvas)
        {
            // Setting Variables
            uiCanvas = _uiCanvas;
        }

        public void Init(GameController _gameController, EventService _eventService)
        {
            // Setting Elements
            uiController = new UIController(uiCanvas, _gameController, _eventService);
        }

        public void Destroy()
        {
            uiController.Destroy();
        }

        // Getters
        public UIController GetUIController() => uiController;
    }
}