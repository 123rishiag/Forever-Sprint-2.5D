using ServiceLocator.Controls;
using ServiceLocator.Sound;
using ServiceLocator.UI;
using ServiceLocator.Vision;

namespace ServiceLocator.Player
{
    public class PlayerService
    {
        // Private Variables
        private PlayerConfig playerConfig;
        private PlayerController playerController;

        public PlayerService(PlayerConfig _playerConfig)
        {
            // Setting Variables
            playerConfig = _playerConfig;
        }

        public void Init(InputService _inputService, SoundService _soundService, UIService _uiService,
            CameraService _cameraService)
        {
            // Setting Elements
            playerController = new PlayerController(playerConfig.playerData, playerConfig.playerPrefab,
                _inputService, _soundService, _uiService);
        }

        public void Reset()
        {
            playerController.Reset();
        }

        public void FixedUpdate()
        {
            playerController.FixedUpdate();
        }

        public void Update()
        {
            playerController.Update();
        }

        // Getters
        public PlayerController GetPlayerController() => playerController;
    }
}