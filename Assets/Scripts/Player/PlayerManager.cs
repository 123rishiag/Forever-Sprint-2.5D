using UnityEngine;

public class PlayerManager : MonoBehaviour
{
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

    [Header("Sliding Settings")]
    [SerializeField] private float slideDuration;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float groundAboveDetectionDistance;
    [SerializeField] private float groundRightDetectionDistance;

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

    private float slideTimer;

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
            case PlayerState.SLIDE:
                playerAnimator.Play("Slide");
                break;
            case PlayerState.DASH:
                break;
            case PlayerState.CLIMB:
                playerAnimator.Play("Climb");
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
        if (playerState == PlayerState.CLIMB) return;

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

            case PlayerState.DASH:
                break;

            case PlayerState.CLIMB:
                HandleClimbState();
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
            playerVelocity.y = airJumpForce;
            playerState = PlayerState.AIR_JUMP;
        }
        else if (CanClimb())
        {
            playerState = PlayerState.CLIMB;
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
        else if (CanClimb())
        {
            playerVelocity.y = 0f;
            playerState = PlayerState.CLIMB;
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
        else if (CanClimb())
        {
            playerVelocity.y = 0f;
            playerState = PlayerState.CLIMB;
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
        else if (CanClimb())
        {
            playerVelocity.y = 0f;
            playerState = PlayerState.CLIMB;
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
        // Restarting Slide Timer
        if (!HasGroundAbove() && inputManager.IsSlidePressed && slideTimer <= 0)
        {
            slideTimer = slideDuration; // Reset slide timer
        }

        slideTimer -= Time.deltaTime;
        if (!IsGrounded())
        {
            playerVelocity.y = 0;
            playerState = PlayerState.FALL;
        }
        else if (inputManager.WasJumpPressed && !HasGroundAbove())
        {
            playerVelocity.y = jumpForce;
            playerState = PlayerState.JUMP;
        }
        else if (slideTimer <= 0 && !HasGroundAbove())
        {
            playerState = PlayerState.IDLE;
        }
    }
    private void HandleClimbState()
    {
        if (ClimbFinished())
        {
            transform.position = playerAnimator.bodyPosition;
            transform.position -= new Vector3(0f, characterController.height, 0f);
            playerState = PlayerState.IDLE;
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

    #region GizmosHandling
    private void OnDrawGizmos()
    {
        if (characterController != null)
        {
            DrawClimbDetectionGizmos();
            DrawGroundAboveGizmos();
        }
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
