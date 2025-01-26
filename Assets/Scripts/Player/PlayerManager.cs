using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerState playerState;
    private CharacterController characterController;
    private Animator playerAnimator;

    private Vector3 velocity;
    private Vector3 moveDirection;
    private float rollTimer;

    private float defaultHeight;
    private Vector3 defaultCenter;

    [Header("Inspector Attachments")]
    [SerializeField] private InputManager inputManager;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airJumpForce;
    [SerializeField] private float gravityForce;
    [SerializeField] private float fallThreshold;
    [SerializeField] private float bigFallThreshold;
    [SerializeField] private float rollDuration;

    [Header("Collider Settings")]
    [SerializeField] private float rollingHeightMultiplier;
    [SerializeField] private float slidingHeightMultiplier;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
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
            case PlayerState.SLIDE:
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
            case PlayerState.ROLL:
                playerAnimator.Play("Roll");
                break;
            case PlayerState.KNOCKED:
                break;
        }
    }
    #endregion

    #region MovementHandling
    private void HandleMovement()
    {
        moveDirection = Vector3.right * moveSpeed; // Default movement direction
        velocity.y -= gravityForce * Time.deltaTime; // Apply gravity
        moveDirection.y = velocity.y; // Update vertical movement
        characterController.Move(moveDirection * Time.deltaTime); // Apply movement
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

            case PlayerState.ROLL:
                HandleRollState();
                break;

            case PlayerState.SLIDE:
                HandleSlideState();
                break;

            default:
                break;
        }
    }
    private void HandleIdleState()
    {
        if (inputManager.WasJumpPressed)
        {
            velocity.y = jumpForce;
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
            velocity.y = jumpForce;
            playerState = PlayerState.JUMP;
        }
        else if (inputManager.IsSlidePressed)
        {
            playerState = PlayerState.SLIDE;
        }
    }
    private void HandleJumpState()
    {
        if (inputManager.WasJumpPressed)
        {
            velocity.y = airJumpForce;
            playerState = PlayerState.AIR_JUMP;
        }
        else if (IsFalling(out PlayerState fallState))
        {
            playerState = fallState;
        }
    }
    private void HandleAirJumpState()
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
        if (velocity.y < -bigFallThreshold)
        {
            _fallState = PlayerState.BIG_FALL;
            return true;
        }
        else if (velocity.y < -fallThreshold)
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
