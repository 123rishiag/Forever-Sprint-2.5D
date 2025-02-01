using ServiceLocator.Controls;
using ServiceLocator.Main;
using ServiceLocator.Sound;
using ServiceLocator.UI;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerService : MonoBehaviour
    {
        [Header("Inspector Attachments")]
        [SerializeField] private bool allowGizmos;

        [Header("Gravity Settings")]
        [SerializeField] private float gravityForce;
        [SerializeField] private float fallThreshold;
        [SerializeField] private float bigFallThreshold;
        [SerializeField] private float deadFallThreshold;

        [Header("Ground Detection")]
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float groundAboveDetectionDistance;
        [SerializeField] private float groundRightDetectionDistance;

        [Header("Collider Settings")]
        [SerializeField] private float defaultHeight;
        [SerializeField] private Vector3 defaultCenter;
        [SerializeField] private float rollingHeightMultiplier;
        [SerializeField] private float slidingHeightMultiplier;

        // Private Variables
        private Animator playerAnimator;
        private CharacterController characterController;
        private PlayerData playerData;
        private PlayerState playerState;

        private int currentHealth;

        private float defaultSpeed;
        private float currentSpeed;
        private Vector3 playerVelocity;
        private Vector3 playerDirection;
        private int airJumpCount;
        private float rollTimer;

        private float slideTimer;

        private float dashTimer;

        // Private Services
        private GameController gameController;
        private InputService inputService;
        private SoundService soundService;
        private UIService uiService;

        private void Awake()
        {
            // Setting Components
            playerAnimator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
        }

        public void Init(PlayerData _playerData, GameController _gameController,
            InputService _inputService, SoundService _soundService, UIService _uiService)
        {
            // Setting Variables
            playerData = _playerData;
            currentHealth = playerData.maxHealth;

            defaultSpeed = playerData.moveSpeed * 100;
            currentSpeed = defaultSpeed;
            airJumpCount = 0;

            // Setting Services
            gameController = _gameController;
            inputService = _inputService;
            soundService = _soundService;
            uiService = _uiService;

            // Setting Elements
            uiService.UpdateHealthText(currentHealth);
        }

        public void UpdatePlayer()
        {
            PlayAnimation();
            HandleMovement();
            HandleStates();
            UpdateColliderDimensions();
        }

        #region AnimationHandling
        private void PlayAnimation()
        {
            playerAnimator.SetFloat("Dash Factor", 0);
            switch (playerState)
            {
                case PlayerState.IDLE:
                    playerAnimator.Play("Idle");
                    break;
                case PlayerState.MOVE:
                    playerAnimator.Play("Move");
                    break;
                case PlayerState.JUMP:
                    playerAnimator.Play("Jump");
                    break;
                case PlayerState.AIR_JUMP:
                    playerAnimator.Play("Air Jump");
                    break;
                case PlayerState.FALL:
                    playerAnimator.Play("Fall");
                    break;
                case PlayerState.BIG_FALL:
                    playerAnimator.Play("Fall");
                    break;
                case PlayerState.DEAD_FALL:
                    playerAnimator.Play("Fall");
                    break;
                case PlayerState.ROLL:
                    playerAnimator.Play("Roll");
                    break;
                case PlayerState.SLIDE:
                    playerAnimator.Play("Slide");
                    break;
                case PlayerState.DASH:
                    playerAnimator.SetFloat("Dash Factor", playerData.dashSpeedIncreaseFactor);
                    playerAnimator.Play("Move");
                    break;
                case PlayerState.CLIMB:
                    playerAnimator.Play("Climb");
                    break;
                case PlayerState.KNOCK:
                    playerAnimator.Play("Knock");
                    break;
                case PlayerState.GET_UP:
                    playerAnimator.Play("Get Up");
                    break;
                case PlayerState.DEAD:
                    playerAnimator.Play("Dead");
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region MovementHandling
        private void HandleMovement()
        {
            if (playerState == PlayerState.IDLE || playerState == PlayerState.CLIMB
                 || playerState == PlayerState.KNOCK || playerState == PlayerState.GET_UP) return;

            if (playerState != PlayerState.DASH)
                currentSpeed = defaultSpeed;

            if (playerState == PlayerState.DEAD) playerVelocity.x = 0f;
            else playerVelocity.x = currentSpeed * Time.deltaTime; // Default movement direction

            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

            playerVelocity.y -= gravityForce * Time.deltaTime; // Apply gravity

            playerDirection.x = playerVelocity.x; // Update horizontal movement
            playerDirection.y = playerVelocity.y; // Update vertical movement

            characterController.Move(playerDirection * Time.deltaTime); // Apply movement
        }
        #endregion

        #region StateHandling
        private void HandleStates()
        {
            switch (playerState)
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
                playerVelocity.y = playerData.jumpForce;
                playerState = PlayerState.JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_JUMP);
            }
            else if (inputService.IsSlidePressed && !CanSlide())
            {
                playerState = PlayerState.SLIDE;
                soundService.PlaySoundEffect(SoundType.PLAYER_SLIDE);
            }
            else if (!HasGroundRight())
            {
                playerState = PlayerState.MOVE;
            }
            else
            {
                playerState = PlayerState.IDLE;
            }
        }
        private void HandleMoveState()
        {
            if (inputService.WasJumpPressed)
            {
                playerVelocity.y = playerData.jumpForce;
                playerState = PlayerState.JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_JUMP);
            }
            else if (inputService.IsSlidePressed)
            {
                playerState = PlayerState.SLIDE;
                soundService.PlaySoundEffect(SoundType.PLAYER_SLIDE);
            }
            else if (IsGrounded() && inputService.IsDashPressed)
            {
                currentSpeed *= playerData.dashSpeedIncreaseFactor;
                playerState = PlayerState.DASH;
            }
            else if (!IsGrounded())
            {
                playerVelocity.y = 0;
                playerState = PlayerState.FALL;
            }
            else if (HasGroundRight())
            {
                playerState = PlayerState.IDLE;
            }
        }
        private void HandleJumpState()
        {
            if (inputService.WasJumpPressed && airJumpCount < playerData.airJumpAllowed)
            {
                airJumpCount++;
                playerVelocity.y = playerData.airJumpForce;
                playerState = PlayerState.AIR_JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_AIR_JUMP);
            }
            else if (CanClimb())
            {
                playerVelocity.y = 0f;
                transform.position += new Vector3(0.30f, 0f, 0f);
                playerState = PlayerState.CLIMB;
            }
            else if (IsFalling(out PlayerState fallState))
            {
                playerState = fallState;
            }
        }
        private void HandleAirJumpState()
        {
            if (inputService.WasJumpPressed && airJumpCount < playerData.airJumpAllowed)
            {
                airJumpCount++;
                playerVelocity.y = playerData.airJumpForce;
                playerState = PlayerState.AIR_JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_AIR_JUMP);
            }
            else if (IsGrounded())
            {
                playerState = PlayerState.IDLE;
            }
            else if (CanClimb())
            {
                playerVelocity.y = 0f;
                transform.position += new Vector3(0.30f, 0f, 0f);
                playerState = PlayerState.CLIMB;
            }
            else if (IsFalling(out PlayerState fallState))
            {
                playerState = fallState;
            }
        }
        private void HandleFallState()
        {
            if (inputService.WasJumpPressed && airJumpCount < playerData.airJumpAllowed)
            {
                airJumpCount++;
                playerVelocity.y = playerData.airJumpForce;
                playerState = PlayerState.AIR_JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_AIR_JUMP);
            }
            else if (IsGrounded())
            {
                playerState = PlayerState.IDLE;
            }
            else if (CanClimb())
            {
                playerVelocity.y = 0f;
                transform.position += new Vector3(0.30f, -0.14f, 0f);
                playerState = PlayerState.CLIMB;
            }
            else if (IsFalling(out PlayerState fallState))
            {
                playerState = fallState;
            }
        }
        private void HandleBigFallState()
        {
            if (inputService.WasJumpPressed && airJumpCount < playerData.airJumpAllowed)
            {
                airJumpCount++;
                playerVelocity.y = playerData.airJumpForce;
                playerState = PlayerState.AIR_JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_AIR_JUMP);
            }
            else if (IsGrounded())
            {
                playerVelocity.y = 0f;
                playerState = PlayerState.ROLL;
                rollTimer = playerData.rollDuration;
            }
            else if (CanClimb())
            {
                playerVelocity.y = 0f;
                transform.position += new Vector3(0.30f, -0.14f, 0f);
                playerState = PlayerState.CLIMB;
            }
            else if (IsFalling(out PlayerState fallState))
            {
                playerState = fallState;
            }
        }
        private void HandleDeadFallState()
        {
            if (inputService.WasJumpPressed && airJumpCount < playerData.airJumpAllowed)
            {
                airJumpCount++;
                playerVelocity.y = playerData.airJumpForce;
                playerState = PlayerState.AIR_JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_AIR_JUMP);
            }
            else if (IsGrounded())
            {
                --currentHealth;
                uiService.UpdateHealthText(currentHealth);
                if (currentHealth > 0)
                {
                    playerState = PlayerState.KNOCK;
                    soundService.PlaySoundEffect(SoundType.PLAYER_KNOCK);
                }
                else
                {
                    playerState = PlayerState.DEAD;
                    soundService.PlaySoundEffect(SoundType.PLAYER_DEAD);
                }
            }
            else if (CanClimb())
            {
                playerVelocity.y = 0f;
                transform.position += new Vector3(0.30f, -0.14f, 0f);
                playerState = PlayerState.CLIMB;
            }
        }
        private void HandleRollState()
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0 && HasGroundAbove())
            {
                playerState = PlayerState.SLIDE;
                soundService.PlaySoundEffect(SoundType.PLAYER_SLIDE);
            }
            else if (rollTimer <= 0)
            {
                playerState = PlayerState.IDLE;
            }
            else if (!IsGrounded())
            {
                playerVelocity.y = 0f;
                playerState = PlayerState.FALL;
            }
        }
        private void HandleSlideState()
        {
            // Restarting Slide Timer
            if (!HasGroundAbove() && inputService.IsSlidePressed && slideTimer <= 0)
            {
                slideTimer = playerData.slideDuration; // Reset slide timer
            }

            slideTimer -= Time.deltaTime;
            if (inputService.WasJumpPressed && !HasGroundAbove())
            {
                playerVelocity.y = playerData.jumpForce;
                playerState = PlayerState.JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_JUMP);
            }
            else if (!IsGrounded())
            {
                playerVelocity.y = 0;
                playerState = PlayerState.FALL;
            }
            else if (slideTimer <= 0 && !HasGroundAbove())
            {
                playerState = PlayerState.IDLE;
            }
            else if (HasGroundRight())
            {
                playerState = PlayerState.IDLE;
            }
        }
        private void HandleDashState()
        {
            // Restarting Dash Timer
            if (!HasGroundRight() && inputService.IsDashPressed && dashTimer <= 0)
            {
                dashTimer = playerData.dashDuration; // Reset dash timer
            }

            dashTimer -= Time.deltaTime;
            if (inputService.WasJumpPressed && !HasGroundAbove())
            {
                playerVelocity.y = playerData.jumpForce;
                playerState = PlayerState.JUMP;
                soundService.PlaySoundEffect(SoundType.PLAYER_JUMP);
            }
            else if (inputService.IsSlidePressed)
            {
                playerState = PlayerState.SLIDE;
                soundService.PlaySoundEffect(SoundType.PLAYER_SLIDE);
            }
            else if (!IsGrounded())
            {
                playerVelocity.y = 0;
                playerState = PlayerState.FALL;
            }
            else if (HasGroundRight())
            {
                --currentHealth;
                uiService.UpdateHealthText(currentHealth);
                if (currentHealth > 0)
                {
                    playerState = PlayerState.KNOCK;
                    soundService.PlaySoundEffect(SoundType.PLAYER_KNOCK);
                }
                else
                {
                    playerState = PlayerState.DEAD;
                    soundService.PlaySoundEffect(SoundType.PLAYER_DEAD);
                }
            }
            else if (dashTimer <= 0)
            {
                dashTimer = playerData.dashDuration;
                playerState = PlayerState.IDLE;
            }
        }
        private void HandleClimbState()
        {
            if (ClimbFinished())
            {
                transform.position = playerAnimator.bodyPosition;
                transform.position -= new Vector3(0f, characterController.height, 0f);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                playerState = PlayerState.IDLE;
            }
        }
        private void HandleKnockState()
        {
            if (KnockFinished())
            {
                transform.position = playerAnimator.bodyPosition;
                playerState = PlayerState.GET_UP;
            }
        }
        private void HandleGetUpState()
        {
            if (GetUpFinished())
            {
                transform.position = playerAnimator.bodyPosition;
                transform.position -= new Vector3(0f, 1.5f, 0f);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                playerState = PlayerState.IDLE;
            }
        }
        private void HandleDeadState()
        {
            if (DeadFinished())
            {
                gameController.GameOver();
            }
        }
        #endregion

        #region CollisonHandling
        private void UpdateColliderDimensions()
        {
            switch (playerState)
            {
                case PlayerState.ROLL:
                    characterController.height = defaultHeight * rollingHeightMultiplier;
                    characterController.center = new Vector3(defaultCenter.x, defaultHeight * rollingHeightMultiplier / 2, defaultCenter.z);
                    break;
                case PlayerState.SLIDE:
                    characterController.height = defaultHeight * slidingHeightMultiplier;
                    characterController.center = new Vector3(defaultCenter.x, defaultHeight * slidingHeightMultiplier / 2, defaultCenter.z);
                    break;
                default:
                    characterController.height = defaultHeight;
                    characterController.center = defaultCenter;
                    break;
            }
        }
        #endregion

        #region Getters
        private bool ClimbFinished()
        {
            return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Climb") &&
                   playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
        private bool KnockFinished()
        {
            return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Knock") &&
                   playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
        private bool GetUpFinished()
        {
            return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Get Up") &&
                   playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
        private bool DeadFinished()
        {
            return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dead") &&
                   playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
        private bool CanSlide()
        {
            // Defining vertical offsets
            float centerOffsetHeight = characterController.height / 2; // Center height of the character
            float legOffsetHeight = 0.1f; // Leg height near the ground

            // Defining the ray origins and direction
            Vector3 centerRayOrigin = transform.position + Vector3.up * centerOffsetHeight;
            Vector3 legRayOrigin = transform.position + Vector3.up * legOffsetHeight;
            Vector3 rayDirection = transform.forward;

            // Checking for ground detection
            bool isGroundInFrontCenter = Physics.Raycast(centerRayOrigin, rayDirection, groundRightDetectionDistance);
            bool isGroundInFrontLeg = Physics.Raycast(legRayOrigin, rayDirection, groundRightDetectionDistance);

            return isGroundInFrontCenter && isGroundInFrontLeg;
        }
        private bool CanClimb()
        {
            // Defining the ray length and vertical offset
            float baseHeight = characterController.height * 1.25f; // Base height above the player
            float verticalOffset = 0.1f; // Small vertical offset between the rays

            // Defining the ray origin and direction
            Vector3 upperRayOrigin = transform.position + Vector3.up * (baseHeight + verticalOffset);
            Vector3 lowerRayOrigin = transform.position + Vector3.up * baseHeight;
            Vector3 rayDirection = transform.forward;

            // Performing Raycasts
            bool isUpperRayNonGround = !Physics.Raycast(upperRayOrigin, rayDirection, groundRightDetectionDistance, groundLayerMask);
            bool isLowerRayGround = Physics.Raycast(lowerRayOrigin, rayDirection, groundRightDetectionDistance, groundLayerMask);

            // Climbing trigger condition
            return isUpperRayNonGround && isLowerRayGround;
        }
        private bool HasGroundRight()
        {
            // Defining the box dimensions
            Vector3 boxSize = new Vector3(characterController.radius * 2, characterController.height - 0.1f,
                groundRightDetectionDistance);

            // Calculating the center of the boxcast
            Vector3 boxCenter = transform.position + new Vector3(0f, 0.1f, 0f) + Vector3.up * (characterController.height / 2);

            // Performing the boxcast
            if (Physics.BoxCast(boxCenter, boxSize / 2, transform.forward, out RaycastHit hit, transform.rotation,
                groundRightDetectionDistance, groundLayerMask))
            {
                // Ground detected
                return true;
            }

            return false;
        }
        private bool HasGroundAbove()
        {
            RaycastHit hit;
            Vector3 rayOrigin = transform.position + Vector3.up * characterController.height;

            if (Physics.Raycast(rayOrigin, Vector3.up, out hit, groundAboveDetectionDistance, groundLayerMask))
            {
                // Ground detected
                return true;
            }

            return false;
        }
        private bool IsGrounded()
        {
            return characterController.isGrounded;
        }
        private bool IsFalling(out PlayerState _fallState)
        {
            if (playerVelocity.y < -deadFallThreshold)
            {
                _fallState = PlayerState.DEAD_FALL;
                return true;
            }
            else if (playerVelocity.y < -bigFallThreshold)
            {
                _fallState = PlayerState.BIG_FALL;
                return true;
            }
            else if (playerVelocity.y < -fallThreshold)
            {
                _fallState = PlayerState.FALL;
                return true;
            }

            _fallState = playerState;
            return false;
        }
        #endregion

        #region GizmosHandling
        private void OnDrawGizmos()
        {
            if (characterController != null && allowGizmos)
            {
                DrawSlideDetectionGizmos();
                DrawClimbDetectionGizmos();
                DrawGroundRightGizmos();
                DrawGroundAboveGizmos();
            }
        }
        private void DrawSlideDetectionGizmos()
        {
            // Defining vertical offsets
            float centerOffsetHeight = characterController.height / 2; // Center height of the character
            float legOffsetHeight = 0.1f; // Leg height near the ground

            // Defining the ray origins and direction
            Vector3 centerRayOrigin = transform.position + Vector3.up * centerOffsetHeight;
            Vector3 legRayOrigin = transform.position + Vector3.up * legOffsetHeight;
            Vector3 rayDirection = transform.forward;

            // Checking for ground detection
            bool isGroundInFrontCenter = Physics.Raycast(centerRayOrigin, rayDirection, groundRightDetectionDistance);
            bool isGroundInFrontLeg = Physics.Raycast(legRayOrigin, rayDirection, groundRightDetectionDistance);

            // Drawing rays for visualization
            Gizmos.color = isGroundInFrontCenter ? Color.green : Color.red;
            Gizmos.DrawRay(centerRayOrigin, rayDirection * groundRightDetectionDistance);

            Gizmos.color = isGroundInFrontLeg ? Color.green : Color.red;
            Gizmos.DrawRay(legRayOrigin, rayDirection * groundRightDetectionDistance);

            // Drawing spheres at the end of the rays for better visibility
            Vector3 centerRayEnd = centerRayOrigin + rayDirection * groundRightDetectionDistance;
            Vector3 legRayEnd = legRayOrigin + rayDirection * groundRightDetectionDistance;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(centerRayEnd, 0.1f);
            Gizmos.DrawWireSphere(legRayEnd, 0.1f);
        }
        private void DrawClimbDetectionGizmos()
        {
            // Defining the ray length and vertical offset
            float baseHeight = characterController.height * 1.25f; // Base height above the player
            float verticalOffset = 0.1f; // Small vertical offset between the rays

            // Defining the ray origin and direction
            Vector3 upperRayOrigin = transform.position + Vector3.up * (baseHeight + verticalOffset);
            Vector3 lowerRayOrigin = transform.position + Vector3.up * baseHeight;
            Vector3 rayDirection = transform.forward;

            // Calculating the end positions of the rays
            Vector3 upperRayEnd = upperRayOrigin + rayDirection * groundRightDetectionDistance;
            Vector3 lowerRayEnd = lowerRayOrigin + rayDirection * groundRightDetectionDistance;

            // Drawing the upper ray and sphere at the end of the ray
            Gizmos.color = Color.red;
            Gizmos.DrawRay(upperRayOrigin, rayDirection * groundRightDetectionDistance);
            Gizmos.DrawWireSphere(upperRayEnd, 0.1f);

            // Drawing the lower ray and sphere at the end of the ray
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(lowerRayOrigin, rayDirection * groundRightDetectionDistance);
            Gizmos.DrawWireSphere(lowerRayEnd, 0.1f);
        }
        private void DrawGroundRightGizmos()
        {
            // Defining the box dimensions
            Vector3 boxSize = new Vector3(characterController.radius * 2, characterController.height - 0.1f, groundRightDetectionDistance);

            // Calculating the center of the boxcast
            Vector3 boxCenter = transform.position + new Vector3(0f, 0.1f, 0f) + Vector3.up * (characterController.height / 2);

            // Setting Gizmo color based on whether a ground is detected
            Gizmos.color = HasGroundRight() ? Color.red : Color.green;

            // Drawing the box representation in the Scene view
            Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, boxSize);
        }
        private void DrawGroundAboveGizmos()
        {
            // Defining the ray origin and direction
            Vector3 rayOrigin = transform.position + Vector3.up * characterController.height;
            Vector3 rayDirection = Vector3.up * groundAboveDetectionDistance;

            // Setting Gizmo color based on whether a ground is detected
            Gizmos.color = HasGroundAbove() ? Color.red : Color.green;

            // Drawing the ray in the Scene view
            Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection);

            // Drawing a sphere at the endpoint for clarity
            Gizmos.DrawWireSphere(rayOrigin + rayDirection, 0.1f);
        }
        #endregion
    }
}