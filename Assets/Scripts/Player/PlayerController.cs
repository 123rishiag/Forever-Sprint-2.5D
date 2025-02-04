using ServiceLocator.Controls;
using ServiceLocator.Sound;
using ServiceLocator.UI;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerController
    {
        // Private Variables
        private PlayerModel playerModel;
        private PlayerView playerView;

        private Vector3 playerDefaultPosition;
        private Vector3 playerVelocity;
        private Vector3 playerDirection;

        private int currentHealth;
        private float defaultSpeed;
        private float currentSpeed;
        private int airJumpCount;
        private float rollTimer;
        private float slideTimer;
        private float dashTimer;

        // Private Services
        private InputService inputService;
        private SoundService soundService;
        private UIService uiService;

        public PlayerController(PlayerData _playerData, PlayerView _playerPrefab,
            InputService _inputService, SoundService _soundService, UIService _uiService)
        {
            // Setting Variables
            playerModel = new PlayerModel(_playerData);
            playerView = Object.Instantiate(_playerPrefab).GetComponent<PlayerView>();
            playerView.Init(this);

            // Setting Services
            inputService = _inputService;
            soundService = _soundService;
            uiService = _uiService;

            // Setting Elements
            playerDefaultPosition = playerView.transform.position;
            Reset();
        }

        public void Reset()
        {
            playerModel.Reset();
            playerView.SetPosition(playerDefaultPosition);

            // Setting Controller Reset
            currentHealth = playerModel.MaxHealth;
            defaultSpeed = playerModel.MoveSpeed * 100;
            currentSpeed = defaultSpeed;
            airJumpCount = 0;

            uiService.GetUIController().UpdateHealthText(currentHealth);
        }

        public void FixedUpdate()
        {
            HandleMovement();
        }
        public void Update()
        {
            playerView.PlayAnimation();
            HandleStates();
            playerView.UpdateColliderDimensions();
        }

        private void HandleMovement()
        {
            if (playerModel.PlayerState == PlayerState.IDLE || playerModel.PlayerState == PlayerState.CLIMB
                 || playerModel.PlayerState == PlayerState.KNOCK || playerModel.PlayerState == PlayerState.GET_UP) return;

            if (playerModel.PlayerState != PlayerState.DASH)
                currentSpeed = defaultSpeed;

            if (playerModel.PlayerState == PlayerState.DEAD) playerVelocity.x = 0f;
            else playerVelocity.x = currentSpeed * Time.fixedDeltaTime; // Default movement direction

            playerView.transform.position = new Vector3(playerView.transform.position.x, playerView.transform.position.y, 0f);

            playerVelocity.y -= playerModel.GravityForce * Time.fixedDeltaTime; // Apply gravity

            playerDirection.x = playerVelocity.x; // Update horizontal movement
            playerDirection.y = playerVelocity.y; // Update vertical movement

            playerView.GetCharacterController().Move(playerDirection * Time.fixedDeltaTime); // Apply movement
        }

        #region StateHandling
        private void HandleStates()
        {
            switch (playerModel.PlayerState)
            {
                case PlayerState.IDLE:
                    HandleIdleState();
                    break;

                case PlayerState.MOVE:
                    HandleMoveState();
                    break;

                case PlayerState.JUMP:
                    HandleJumpState();
                    break;

                case PlayerState.AIR_JUMP:
                    HandleAirJumpState();
                    break;

                case PlayerState.FALL:
                    HandleFallState();
                    break;

                case PlayerState.BIG_FALL:
                    HandleBigFallState();
                    break;

                case PlayerState.DEAD_FALL:
                    HandleDeadFallState();
                    break;

                case PlayerState.ROLL:
                    HandleRollState();
                    break;

                case PlayerState.SLIDE:
                    HandleSlideState();
                    break;

                case PlayerState.DASH:
                    HandleDashState();
                    break;

                case PlayerState.CLIMB:
                    HandleClimbState();
                    break;

                case PlayerState.KNOCK:
                    HandleKnockState();
                    break;

                case PlayerState.GET_UP:
                    HandleGetUpState();
                    break;

                case PlayerState.DEAD:
                    HandleDeadState();
                    break;

                default:
                    break;
            }
        }
        private void HandleIdleState()
        {
            airJumpCount = 0;
            if (inputService.WasJumpPressed)
            {
                playerVelocity.y = playerModel.JumpForce;
                playerModel.PlayerState = PlayerState.JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_JUMP);
            }
            else if (inputService.IsSlidePressed && !playerView.CanSlide())
            {
                playerModel.PlayerState = PlayerState.SLIDE;
                soundService.PlaySoundEffect(SoundType.PLAYER_SLIDE);
            }
            else if (!playerView.HasGroundRight())
            {
                playerModel.PlayerState = PlayerState.MOVE;
            }
            else
            {
                playerModel.PlayerState = PlayerState.IDLE;
            }
        }
        private void HandleMoveState()
        {
            if (inputService.WasJumpPressed)
            {
                playerVelocity.y = playerModel.JumpForce;
                playerModel.PlayerState = PlayerState.JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_JUMP);
            }
            else if (inputService.IsSlidePressed)
            {
                playerModel.PlayerState = PlayerState.SLIDE;
                soundService.PlaySoundEffect(SoundType.PLAYER_SLIDE);
            }
            else if (playerView.IsGrounded() && inputService.IsDashPressed)
            {
                currentSpeed *= playerModel.DashSpeedIncreaseFactor;
                playerModel.PlayerState = PlayerState.DASH;
            }
            else if (!playerView.IsGrounded())
            {
                playerVelocity.y = 0;
                playerModel.PlayerState = PlayerState.FALL;
            }
            else if (playerView.HasGroundRight())
            {
                playerModel.PlayerState = PlayerState.IDLE;
            }
        }
        private void HandleJumpState()
        {
            if (inputService.WasJumpPressed && airJumpCount < playerModel.AirJumpAllowed)
            {
                airJumpCount++;
                playerVelocity.y = playerModel.AirJumpForce;
                playerModel.PlayerState = PlayerState.AIR_JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_AIR_JUMP);
            }
            else if (playerView.CanClimb())
            {
                playerVelocity.y = 0f;
                playerView.transform.position += new Vector3(0.30f, 0f, 0f);
                playerModel.PlayerState = PlayerState.CLIMB;
            }
            else if (IsFalling(out PlayerState fallState))
            {
                playerModel.PlayerState = fallState;
            }
        }
        private void HandleAirJumpState()
        {
            if (inputService.WasJumpPressed && airJumpCount < playerModel.AirJumpAllowed)
            {
                airJumpCount++;
                playerVelocity.y = playerModel.AirJumpForce;
                playerModel.PlayerState = PlayerState.AIR_JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_AIR_JUMP);
            }
            else if (playerView.IsGrounded())
            {
                playerModel.PlayerState = PlayerState.IDLE;
            }
            else if (playerView.CanClimb())
            {
                playerVelocity.y = 0f;
                playerView.transform.position += new Vector3(0.30f, 0f, 0f);
                playerModel.PlayerState = PlayerState.CLIMB;
            }
            else if (IsFalling(out PlayerState fallState))
            {
                playerModel.PlayerState = fallState;
            }
        }
        private void HandleFallState()
        {
            if (inputService.WasJumpPressed && airJumpCount < playerModel.AirJumpAllowed)
            {
                airJumpCount++;
                playerVelocity.y = playerModel.AirJumpForce;
                playerModel.PlayerState = PlayerState.AIR_JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_AIR_JUMP);
            }
            else if (playerView.IsGrounded())
            {
                playerModel.PlayerState = PlayerState.IDLE;
            }
            else if (playerView.CanClimb())
            {
                playerVelocity.y = 0f;
                playerView.transform.position += new Vector3(0.30f, -0.14f, 0f);
                playerModel.PlayerState = PlayerState.CLIMB;
            }
            else if (IsFalling(out PlayerState fallState))
            {
                playerModel.PlayerState = fallState;
            }
        }
        private void HandleBigFallState()
        {
            if (inputService.WasJumpPressed && airJumpCount < playerModel.AirJumpAllowed)
            {
                airJumpCount++;
                playerVelocity.y = playerModel.AirJumpForce;
                playerModel.PlayerState = PlayerState.AIR_JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_AIR_JUMP);
            }
            else if (playerView.IsGrounded())
            {
                playerVelocity.y = 0f;
                playerModel.PlayerState = PlayerState.ROLL;
                rollTimer = playerModel.RollDuration;
            }
            else if (playerView.CanClimb())
            {
                playerVelocity.y = 0f;
                playerView.transform.position += new Vector3(0.30f, -0.14f, 0f);
                playerModel.PlayerState = PlayerState.CLIMB;
            }
            else if (IsFalling(out PlayerState fallState))
            {
                playerModel.PlayerState = fallState;
            }
        }
        private void HandleDeadFallState()
        {
            if (inputService.WasJumpPressed && airJumpCount < playerModel.AirJumpAllowed)
            {
                airJumpCount++;
                playerVelocity.y = playerModel.AirJumpForce;
                playerModel.PlayerState = PlayerState.AIR_JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_AIR_JUMP);
            }
            else if (playerView.IsGrounded())
            {
                --currentHealth;
                uiService.GetUIController().UpdateHealthText(currentHealth);
                if (currentHealth > 0)
                {
                    playerModel.PlayerState = PlayerState.KNOCK;
                    soundService.PlaySoundEffect(SoundType.PLAYER_KNOCK);
                }
                else
                {
                    playerModel.PlayerState = PlayerState.DEAD;
                    soundService.PlaySoundEffect(SoundType.PLAYER_DEAD);
                }
            }
            else if (playerView.CanClimb())
            {
                playerVelocity.y = 0f;
                playerView.transform.position += new Vector3(0.30f, -0.14f, 0f);
                playerModel.PlayerState = PlayerState.CLIMB;
            }
        }
        private void HandleRollState()
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0 && playerView.HasGroundAbove())
            {
                playerModel.PlayerState = PlayerState.SLIDE;
                soundService.PlaySoundEffect(SoundType.PLAYER_SLIDE);
            }
            else if (rollTimer <= 0)
            {
                playerModel.PlayerState = PlayerState.IDLE;
            }
            else if (!playerView.IsGrounded())
            {
                playerVelocity.y = 0f;
                playerModel.PlayerState = PlayerState.FALL;
            }
        }
        private void HandleSlideState()
        {
            // Restarting Slide Timer
            if (!playerView.HasGroundAbove() && inputService.IsSlidePressed && slideTimer <= 0)
            {
                slideTimer = playerModel.SlideDuration; // Reset slide timer
            }

            slideTimer -= Time.deltaTime;
            if (inputService.WasJumpPressed && !playerView.HasGroundAbove())
            {
                playerVelocity.y = playerModel.JumpForce;
                playerModel.PlayerState = PlayerState.JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_JUMP);
            }
            else if (!playerView.IsGrounded())
            {
                playerVelocity.y = 0;
                playerModel.PlayerState = PlayerState.FALL;
            }
            else if (slideTimer <= 0 && !playerView.HasGroundAbove())
            {
                playerModel.PlayerState = PlayerState.IDLE;
            }
            else if (playerView.HasGroundRight())
            {
                playerModel.PlayerState = PlayerState.IDLE;
            }
        }
        private void HandleDashState()
        {
            // Restarting Dash Timer
            if (!playerView.HasGroundRight() && inputService.IsDashPressed && dashTimer <= 0)
            {
                dashTimer = playerModel.DashDuration; // Reset dash timer
            }

            dashTimer -= Time.deltaTime;
            if (inputService.WasJumpPressed && !playerView.HasGroundAbove())
            {
                playerVelocity.y = playerModel.JumpForce;
                playerModel.PlayerState = PlayerState.JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_JUMP);
            }
            else if (inputService.IsSlidePressed)
            {
                playerModel.PlayerState = PlayerState.SLIDE;
                soundService.PlaySoundEffect(SoundType.PLAYER_SLIDE);
            }
            else if (!playerView.IsGrounded())
            {
                playerVelocity.y = 0;
                playerModel.PlayerState = PlayerState.FALL;
            }
            else if (playerView.HasGroundRight())
            {
                --currentHealth;
                uiService.GetUIController().UpdateHealthText(currentHealth);
                if (currentHealth > 0)
                {
                    playerModel.PlayerState = PlayerState.KNOCK;
                    soundService.PlaySoundEffect(SoundType.PLAYER_KNOCK);
                }
                else
                {
                    playerModel.PlayerState = PlayerState.DEAD;
                    soundService.PlaySoundEffect(SoundType.PLAYER_DEAD);
                }
            }
            else if (dashTimer <= 0)
            {
                dashTimer = playerModel.DashDuration;
                playerModel.PlayerState = PlayerState.IDLE;
            }
        }
        private void HandleClimbState()
        {
            if (playerView.ClimbFinished())
            {
                playerView.transform.position = playerView.GetAnimator().bodyPosition;
                playerView.transform.position -= new Vector3(0f, playerView.GetCharacterController().height, 0f);
                playerView.transform.position = new Vector3(playerView.transform.position.x, playerView.transform.position.y, 0f);
                playerModel.PlayerState = PlayerState.IDLE;
            }
        }
        private void HandleKnockState()
        {
            if (playerView.KnockFinished())
            {
                playerView.transform.position = playerView.GetAnimator().bodyPosition;
                playerModel.PlayerState = PlayerState.GET_UP;
            }
        }
        private void HandleGetUpState()
        {
            if (playerView.GetUpFinished())
            {
                playerView.transform.position = playerView.GetAnimator().bodyPosition;
                playerView.transform.position -= new Vector3(0f, 1.5f, 0f);
                playerView.transform.position = new Vector3(playerView.transform.position.x, playerView.transform.position.y, 0f);
                playerModel.PlayerState = PlayerState.IDLE;
            }
        }
        private void HandleDeadState()
        { }
        #endregion

        #region Getters
        public bool IsAlive()
        {
            if (playerView.DeadFinished() && currentHealth <= 0)
            {
                return false;
            }
            return true;
        }
        private bool IsFalling(out PlayerState _fallState)
        {
            if (playerVelocity.y < -playerModel.DeadFallThreshold)
            {
                _fallState = PlayerState.DEAD_FALL;
                return true;
            }
            else if (playerVelocity.y < -playerModel.BigFallThreshold)
            {
                _fallState = PlayerState.BIG_FALL;
                return true;
            }
            else if (playerVelocity.y < -playerModel.FallThreshold)
            {
                _fallState = PlayerState.FALL;
                return true;
            }

            _fallState = playerModel.PlayerState;
            return false;
        }
        public Transform GetTransform() => playerView.transform;
        public Transform GetFollowTransform() => playerView.GetFollowTransform();
        public PlayerModel GetModel() => playerModel;
        #endregion
    }
}