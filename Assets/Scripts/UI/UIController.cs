using ServiceLocator.Main;

namespace ServiceLocator.UI
{
    public class UIController
    {
        // Private Variables
        private UIView uiView;
        private GameController gameController;

        public UIController(UIView _uiCanvas, GameController _gameController)
        {
            // Setting Variables
            uiView = _uiCanvas.GetComponent<UIView>();
            gameController = _gameController;

            // Adding Listeners
            uiView.pauseMenuResumeButton.onClick.AddListener(gameController.PlayGame);
            uiView.pauseMenuMainMenuButton.onClick.AddListener(gameController.MainMenu);

            uiView.gameOverMenuRestartButton.onClick.AddListener(gameController.RestartGame);
            uiView.gameOverMenuMainMenuButton.onClick.AddListener(gameController.MainMenu);

            uiView.mainMenuPlayButton.onClick.AddListener(gameController.PlayGame);
            uiView.mainMenuQuitButton.onClick.AddListener(gameController.QuitGame);
            uiView.mainMenuMuteButton.onClick.AddListener(gameController.MuteGame);
        }
        public void Destroy()
        {
            // Removing Listeners
            uiView.pauseMenuResumeButton.onClick.RemoveListener(gameController.PlayGame);
            uiView.pauseMenuMainMenuButton.onClick.RemoveListener(gameController.MainMenu);

            uiView.gameOverMenuRestartButton.onClick.RemoveListener(gameController.RestartGame);
            uiView.gameOverMenuMainMenuButton.onClick.RemoveListener(gameController.MainMenu);

            uiView.mainMenuPlayButton.onClick.RemoveListener(gameController.PlayGame);
            uiView.mainMenuQuitButton.onClick.RemoveListener(gameController.QuitGame);
            uiView.mainMenuMuteButton.onClick.RemoveListener(gameController.MuteGame);
        }

        // Setters
        public void UpdateHealthText(int _health)
        {
            uiView.UpdateHealthText(_health);
        }
        public void UpdateScoreText(int _score)
        {
            uiView.UpdateScoreText(_score);
        }
        public void SetMuteButtonText(bool _isMute)
        {
            uiView.SetMuteButtonText(_isMute);
        }
        public void EnablePauseMenuPanel(bool _flag) => uiView.pauseMenuPanel.SetActive(_flag);
        public void EnableGameOverMenuPanel(bool _flag) => uiView.gameOverMenuPanel.SetActive(_flag);
        public void EnableMainMenuPanel(bool _flag) => uiView.mainMenuPanel.SetActive(_flag);

        // Getters
        public bool IsPauseMenuPanelEnabled() => uiView.pauseMenuPanel.activeInHierarchy;
    }
}