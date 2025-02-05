using ServiceLocator.Controls;
using ServiceLocator.Event;

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

        public void Init(EventService _eventService, InputService _inputService)
        {
            // Setting Elements
            playerController = new PlayerController(playerConfig.playerData, playerConfig.playerPrefab,
                _eventService, _inputService);
        }

        public void Destroy() => playerController.Destroy();
        public void Reset() => playerController.Reset();
        public void FixedUpdate() => playerController.FixedUpdate();
        public void Update() => playerController.Update();

        // Getters
        public PlayerController GetPlayerController() => playerController;
    }
}