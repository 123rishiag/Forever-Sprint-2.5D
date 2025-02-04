using ServiceLocator.Controls;
using ServiceLocator.Sound;
using ServiceLocator.UI;
using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerController : IStateOwner<PlayerController>
    {
        // Private Variables
        private PlayerModel playerModel;
        private PlayerView playerView;

        private Vector3 playerDefaultPosition;
        private Vector3 playerDirection;
        public Vector3 PlayerVelocity;

        public int CurrentHealth { get; set; }
        public float DefaultSpeed { get; set; }
        public float CurrentSpeed { get; set; }
        public int AirJumpCount { get; set; }
        public float RollTimer { get; set; }
        public float SlideTimer { get; set; }
        public float DashTimer { get; set; }

        public PlayerController Owner { get; set; }
        private PlayerStateMachine playerStateMachine;

        // Private Services
        public InputService InputService { get; private set; }
        public SoundService SoundService { get; private set; }
        public UIService UIService { get; private set; }

        public PlayerController(PlayerData _playerData, PlayerView _playerPrefab,
            InputService _inputService, SoundService _soundService, UIService _uiService)
        {
            // Setting Variables
            playerModel = new PlayerModel(_playerData);
            playerView = Object.Instantiate(_playerPrefab).GetComponent<PlayerView>();

            // Setting Services
            InputService = _inputService;
            SoundService = _soundService;
            UIService = _uiService;

            // Setting Elements
            playerDefaultPosition = playerView.transform.position;
            CreateStateMachine();
            Reset();
        }

        private void CreateStateMachine()
        {
            Owner = this;
            playerStateMachine = new PlayerStateMachine(this);
        }

        public void Reset()
        {
            playerView.SetPosition(playerDefaultPosition);

            // Setting Controller Reset
            CurrentHealth = playerModel.MaxHealth;
            UIService.GetUIController().UpdateHealthText(CurrentHealth);

            DefaultSpeed = playerModel.MoveSpeed * 100;
            CurrentSpeed = DefaultSpeed;
            AirJumpCount = 0;

            playerStateMachine.ChangeState(PlayerState.IDLE);
        }

        public void FixedUpdate()
        {
            playerStateMachine.FixedUpdate();
            HandleMovement();
        }
        public void Update()
        {
            playerView.UpdateColliderDimensions(playerStateMachine.GetCurrentState());
            playerStateMachine.Update();
        }

        private void HandleMovement()
        {
            if (playerStateMachine.GetCurrentState() == PlayerState.IDLE ||
                playerStateMachine.GetCurrentState() == PlayerState.CLIMB ||
                playerStateMachine.GetCurrentState() == PlayerState.KNOCK ||
                playerStateMachine.GetCurrentState() == PlayerState.GET_UP) return;

            if (playerStateMachine.GetCurrentState() != PlayerState.DASH)
                CurrentSpeed = DefaultSpeed;

            if (playerStateMachine.GetCurrentState() == PlayerState.DEAD) PlayerVelocity.x = 0f;
            else PlayerVelocity.x = CurrentSpeed * Time.fixedDeltaTime; // Default movement direction

            PlayerVelocity.y -= playerModel.GravityForce * Time.fixedDeltaTime; // Apply gravity

            playerDirection.x = PlayerVelocity.x; // Update horizontal movement
            playerDirection.y = PlayerVelocity.y; // Update vertical movement

            playerView.GetCharacterController().Move(playerDirection * Time.fixedDeltaTime); // Apply movement
        }

        // Setters
        public void SetVelocity(float _velocity) => PlayerVelocity.y = _velocity;
        public void DecreaseHealth()
        {
            --CurrentHealth;
            UIService.GetUIController().UpdateHealthText(CurrentHealth);

            if (CurrentHealth > 0)
            {
                playerStateMachine.ChangeState(PlayerState.KNOCK);
            }
            else
            {
                playerStateMachine.ChangeState(PlayerState.DEAD);
            }
        }

        // Getters
        public bool IsAlive()
        {
            if (playerView.DeadFinished() && CurrentHealth <= 0)
            {
                return false;
            }
            return true;
        }
        public Transform GetTransform() => playerView.transform;
        public Transform GetFollowTransform() => playerView.GetFollowTransform();
        public PlayerModel GetModel() => playerModel;
        public PlayerView GetView() => playerView;
    }
}