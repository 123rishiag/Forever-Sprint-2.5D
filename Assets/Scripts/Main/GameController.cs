using ServiceLocator.Collectible;
using ServiceLocator.Controls;
using ServiceLocator.Level;
using ServiceLocator.Player;
using ServiceLocator.Score;
using ServiceLocator.Sound;
using ServiceLocator.UI;
using ServiceLocator.Vision;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ServiceLocator.Main
{
    public class GameController
    {
        // Private Variables
        private GameService gameService;

        private InputService inputService;
        private CameraService cameraService;
        private SoundService soundService;
        private UIService uiService;
        private ScoreService scoreService;
        private PlayerService playerService;
        private CollectibleService collectibleService;
        private LevelService levelService;

        public GameController(GameService _gameService)
        {
            // Setting Elements
            Time.timeScale = 0f;

            // Setting Variables
            gameService = _gameService;
            CreateServices();
            InjectDependencies();
            Reset();
        }
        private void CreateServices()
        {
            inputService = new InputService();
            cameraService = new CameraService(gameService.virtualCamera);
            soundService = new SoundService(gameService.soundConfig, gameService.bgmSource, gameService.sfxSource);
            uiService = new UIService(gameService.uiCanvas);
            scoreService = new ScoreService();
            playerService = new PlayerService(gameService.playerConfig);
            collectibleService = new CollectibleService(gameService.collectibleConfig, gameService.collectibleParentPanel);
            levelService = new LevelService(gameService.levelConfig, gameService.levelParentPanel);
        }
        private void InjectDependencies()
        {
            inputService.Init();
            // No Camera Service Init
            // No Sound Service Init
            uiService.Init(this);
            scoreService.Init(uiService);
            playerService.Init(inputService, soundService, uiService, cameraService, this);
            collectibleService.Init(playerService);
            levelService.Init(playerService, collectibleService);
        }

        public void Reset()
        {
            // No Input Service Reset
            // No Camera Service Reset
            soundService.Reset();
            // No UI Service Reset
            scoreService.Reset();
            playerService.Reset();
            collectibleService.Reset();
            levelService.Reset();
        }

        public void Destroy()
        {
            inputService.Destroy();
            // No Camera Service Destroy
            // No Sound Service Destroy
            uiService.Destroy();
            // No Score Service Destroy
            // No Player Service Destroy
            collectibleService.Destroy();
            levelService.Destroy();
        }

        public void FixedUpdate()
        {
            // No Input Service Fixed Update
            // No Camera Service Fixed Update
            // No Sound Service Fixed Update
            // No UI Service Fixed Update
            // No Score Service Fixed Update
            playerService.FixedUpdate();
            // No Collectible Service Fixed Update
            // No Collectible Service Fixed Update
        }

        public void Update()
        {
            inputService.Update();
            // No Camera Service Update
            // No Sound Service Update
            // No UI Service Update
            // No Score Service Update
            playerService.Update();
            collectibleService.Update();
            levelService.Update();

            // Checking Elements
            if (inputService.IsEscapePressed)
            {
                PauseGame();
            }
        }

        public void PlayGame()
        {
            Time.timeScale = 1f;
            uiService.GetUIController().EnableMainMenuPanel(false);
            soundService.PlaySoundEffect(SoundType.GAME_PLAY);
        }
        public void PauseGame()
        {
            if (!uiService.GetUIController().IsPauseMenuPanelEnabled())
            {
                Time.timeScale = 0f;
                uiService.GetUIController().EnablePauseMenuPanel(true);
                soundService.PlaySoundEffect(SoundType.GAME_PAUSE);
            }
        }
        public void ResumeGame()
        {
            Time.timeScale = 1f;
            uiService.GetUIController().EnablePauseMenuPanel(false);
            soundService.PlaySoundEffect(SoundType.GAME_PLAY);
        }
        public void RestartGame()
        {
            soundService.PlaySoundEffect(SoundType.GAME_PLAY);
            SceneManager.LoadScene(0); // Reload 0th scene
        }
        public void MainMenu()
        {
            soundService.PlaySoundEffect(SoundType.BUTTON_QUIT);
            SceneManager.LoadScene(0); // Reload 0th scene
        }
        public void GameOver()
        {
            Time.timeScale = 0f;
            uiService.GetUIController().EnableGameOverMenuPanel(true);
            soundService.PlaySoundEffect(SoundType.GAME_OVER);
        }
        public void QuitGame()
        {
            soundService.PlaySoundEffect(SoundType.BUTTON_QUIT);
            Application.Quit();
        }

        public void MuteGame()
        {
            soundService.MuteGame();
            uiService.GetUIController().SetMuteButtonText(soundService.IsMute);
            soundService.PlaySoundEffect(SoundType.BUTTON_CLICK);
        }

        // Getters
        public InputService GetInputService() => inputService;
        public CameraService GetCameraService() => cameraService;
        public SoundService GetSoundService() => soundService;
        public UIService GetUIService() => uiService;
        public ScoreService GetScoreService() => scoreService;
        public PlayerService GetPlayerService() => playerService;
        public CollectibleService GetCollectibleService() => collectibleService;
        public LevelService GetLevelService() => levelService;
    }
}