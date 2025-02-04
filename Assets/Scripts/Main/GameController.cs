using ServiceLocator.Collectible;
using ServiceLocator.Controls;
using ServiceLocator.Level;
using ServiceLocator.Player;
using ServiceLocator.Score;
using ServiceLocator.Sound;
using ServiceLocator.UI;
using ServiceLocator.Utility;
using ServiceLocator.Vision;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ServiceLocator.Main
{
    public class GameController : IStateOwner<GameController>
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

        public GameController Owner { get; set; }
        private GameStateMachine gameStateMachine;

        public GameController(GameService _gameService)
        {
            // Setting Services
            gameService = _gameService;
            CreateServices();

            // Setting Elements
            InjectDependencies();
            CreateStateMachine();
            gameStateMachine.ChangeState(GameState.GAME_START);
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
            playerService.Init(inputService, soundService, uiService, cameraService);
            collectibleService.Init(playerService, scoreService, soundService);
            levelService.Init(playerService, collectibleService);
        }

        private void CreateStateMachine()
        {
            Owner = this;
            gameStateMachine = new GameStateMachine(this);
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
            // No Collectible Service Destroy
            // No Level Service Destroy
        }

        public void Update()
        {
            gameStateMachine.Update();
        }
        public void FixedUpdate()
        {
            gameStateMachine.FixedUpdate();
        }

        public void PlayGame()
        {
            soundService.PlaySoundEffect(SoundType.BUTTON_CLICK);
            gameStateMachine.ChangeState(GameState.GAME_PLAY);
        }
        public void RestartGame()
        {
            soundService.PlaySoundEffect(SoundType.BUTTON_CLICK);
            gameStateMachine.ChangeState(GameState.GAME_RESTART);
        }
        public void MainMenu()
        {
            soundService.PlaySoundEffect(SoundType.BUTTON_QUIT);
            SceneManager.LoadScene(0); // Reload 0th scene
        }

        public void QuitGame()
        {
            soundService.PlaySoundEffect(SoundType.BUTTON_QUIT);
            Application.Quit();
        }

        public void MuteGame()
        {
            uiService.GetUIController().SetMuteButtonText(soundService.IsMute);
            soundService.MuteGame(); // Mute/unmute the game
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

        public GameState GetCurrentState() => gameStateMachine.GetCurrentState();
    }
}