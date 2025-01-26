using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Inspector Attachments")]
    [SerializeField] private InputManager inputManager;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float powerJumpForce;
    [SerializeField] private float gravityForce;
    [SerializeField] private float fallThreshold;
    [SerializeField] private float bigFallThreshold;
    [SerializeField] private float rollDuration;

    [Header("Collider Settings")]
    [SerializeField] private float rollingHeightMultiplier;
    [SerializeField] private float slidingHeightMultiplier;

    // Private Components
    private PlayerState playerState;
    private Animator playerAnimator;
    private CharacterController characterController;

    // Private Variables
    private Vector3 playerVelocity;
    private Vector3 playerDirection;
    private float rollTimer;

    private float defaultHeight;
    private Vector3 defaultCenter;

    private void Awake()
    {
        // Setting Components
        playerAnimator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // Setting Variables
        defaultHeight = characterController.height;
        defaultCenter = characterController.center;
    }

    private void Update()
    {
        PlayAnimation();
        HandleMovement();
        HandleStates();
        UpdateColliderDimensions();
    }

    #region AnimationHandling
    private void PlayAnimation()
    {
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
            case PlayerState.POWER_JUMP:
                playerAnimator.Play("Power Jump");
                break;
            case PlayerState.FALL:
                playerAnimator.Play("Fall");
                break;
            case PlayerState.BIG_FALL:
                playerAnimator.Play("Fall");
                break;
            case PlayerState.ROLL:
                playerAnimator.Play("Roll");
                break;
            case PlayerState.SLIDE:
                break;
            case PlayerState.DASH:
                break;
            case PlayerState.CLIMB:
                break;
            case PlayerState.KNOCK:
                break;
            case PlayerState.DEAD:
                break;
            default:
                break;
        }
    }
    #endregion

    #region MovementHandling
    private void HandleMovement()
    {
        playerDirection = Vector3.right * moveSpeed; // Default movement direction
        playerVelocity.y -= gravityForce * Time.deltaTime; // Apply gravity
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

            case PlayerState.POWER_JUMP:
                HandlePowerJumpState();
                break;

            case PlayerState.FALL:
                HandleFallState();
                break;

            case PlayerState.BIG_FALL:
                HandleBigFallState();
                break;

            case PlayerState.ROLL:
                HandleRollState();
                break;

            case PlayerState.SLIDE:
                HandleSlideState();
                break;

            case PlayerState.DASH:
                break;

            case PlayerState.CLIMB:
                break;

            case PlayerState.KNOCK:
                break;

            case PlayerState.DEAD:
                break;

            default:
                break;
        }
    }
    private void HandleIdleState()
    {
        if (inputManager.WasJumpPressed)
        {
            playerVelocity.y = jumpForce;
            playerState = PlayerState.JUMP;
        }
        else if (inputManager.IsSlidePressed)
        {
            playerState = PlayerState.SLIDE;
        }
        else
        {
            playerState = PlayerState.MOVE;
        }
    }
    private void HandleMoveState()
    {
        if (inputManager.WasJumpPressed)
        {
            playerVelocity.y = jumpForce;
            playerState = PlayerState.JUMP;
        }
        else if (inputManager.IsSlidePressed)
        {
            playerState = PlayerState.SLIDE;
        }
        else if (!IsGrounded())
        {
            playerVelocity.y = 0;
            playerState = PlayerState.FALL;
        }
    }
    private void HandleJumpState()
    {
        if (inputManager.WasJumpPressed)
        {
            playerVelocity.y = powerJumpForce;
            playerState = PlayerState.POWER_JUMP;
        }
        else if (IsFalling(out PlayerState fallState))
        {
            playerState = fallState;
        }
    }
    private void HandlePowerJumpState()
    {
        if (IsGrounded())
        {
            playerState = PlayerState.IDLE;
        }
        else if (IsFalling(out PlayerState fallState))
        {
            playerState = fallState;
        }
    }
    private void HandleFallState()
    {
        if (IsGrounded())
        {
            playerState = PlayerState.IDLE;
        }
        else if (IsFalling(out PlayerState fallState))
        {
            playerState = fallState;
        }
    }
    private void HandleBigFallState()
    {
        if (IsGrounded())
        {
            playerState = PlayerState.ROLL;
            rollTimer = rollDuration;
        }
        else if (IsFalling(out PlayerState fallState))
        {
            playerState = fallState;
        }
    }
    private void HandleRollState()
    {
        rollTimer -= Time.deltaTime;

        if (rollTimer <= 0)
        {
            playerState = PlayerState.IDLE;
        }
    }
    private void HandleSlideState()
    {
        if (!inputManager.IsSlidePressed)
        {
            playerState = PlayerState.IDLE;
        }
    }
    private bool IsGrounded()
    {
        return characterController.isGrounded;
    }
    private bool IsFalling(out PlayerState _fallState)
    {
        if (playerVelocity.y < -bigFallThreshold)
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
}
