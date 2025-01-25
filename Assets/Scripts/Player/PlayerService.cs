using UnityEngine;

public class PlayerService : MonoBehaviour
{
    // Private Services
    [Header("Service Variables")]
    [SerializeField] private InputService inputService;

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float movementSmoothingFactor;

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float airJumpForce;
    [SerializeField] private float jumpDuration;
    [SerializeField] private int consectiveJumpsAllowed;
    [SerializeField] private float jumpSmoothingFactor;
    [SerializeField] private float gravityScaleFactor;

    [Header("Roll Variables")]
    [SerializeField] private float rollDuration;

    // Private Variables
    private CharacterController characterController;
    private Animator playerAnimator;
    private PlayerState playerState;
    private Vector3 playerDirection;

    private bool isMoving;

    private float verticalVelocity;
    private bool wasJumpPressed;
    private bool isJumping;
    private float jumpTimer;
    private int currentJumpCounter;

    private float rollTimer;

    // Animator Hash
    private int xVelocityHash = Animator.StringToHash("xVelocity");
    private int moveIdleHash = Animator.StringToHash("Move/Idle");
    private int yVelocityHash = Animator.StringToHash("yVelocity");
    private int jumpHash = Animator.StringToHash("Jump");
    private int airJumpHash = Animator.StringToHash("Air Jump");
    private int rollHash = Animator.StringToHash("Roll");

    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        playerState = PlayerState.IDLE;
        playerDirection = new Vector3(0f, 0f, 0f);
        jumpTimer = jumpDuration;
        rollTimer = 0;
        currentJumpCounter = 0;
    }
    private void Update()
    {
        PlayerInput();
        ChangeState();
        MoveIdle();
        JumpFall();
        HandleJump();
        HandleRoll();
        PlayAnimations();
    }

    private void FixedUpdate()
    {
        PerformMovement();
        ApplyGravity();
    }

    private void PerformMovement()
    {
        characterController.Move(playerDirection * Time.fixedDeltaTime);
    }
    private void ApplyGravity()
    {
        if (playerState == PlayerState.FALL || playerState == PlayerState.AIR_JUMP_FALL)
            verticalVelocity -= gravityScaleFactor * Time.fixedDeltaTime;
        else
            verticalVelocity = -.5f;
    }

    private void PlayerInput()
    {
        isMoving = inputService.IsMoving;
        isJumping = inputService.IsJumping;
        wasJumpPressed = inputService.WasJumpPressed;
    }

    private void ChangeState()
    {
        if (characterController.isGrounded && rollTimer > 0)
            playerState = PlayerState.ROLL;

        else if (jumpTimer > 0 && isJumping &&
            (currentJumpCounter > 0 && currentJumpCounter < consectiveJumpsAllowed))
            playerState = PlayerState.AIR_JUMP;

        else if (jumpTimer > 0 && isJumping && currentJumpCounter == 0)
            playerState = PlayerState.JUMP;

        else if (!characterController.isGrounded && (currentJumpCounter > 0
            && currentJumpCounter < consectiveJumpsAllowed))
            playerState = PlayerState.AIR_JUMP_FALL;

        else if (!characterController.isGrounded && currentJumpCounter == 0)
            playerState = PlayerState.FALL;

        else if (isMoving)
            playerState = PlayerState.MOVE;

        else
            playerState = PlayerState.IDLE;
    }

    private void MoveIdle()
    {
        float targetSpeed = isMoving ? moveSpeed : 0f;
        playerDirection.x = Mathf.Lerp(playerDirection.x, targetSpeed, Time.deltaTime * movementSmoothingFactor);
    }

    private void JumpFall()
    {
        float targetY;

        if (playerState == PlayerState.AIR_JUMP)
            targetY = airJumpForce;
        else if (playerState == PlayerState.JUMP)
            targetY = jumpForce;
        else
            targetY = verticalVelocity;

        playerDirection.y = Mathf.Lerp(playerDirection.y, targetY, Time.deltaTime * jumpSmoothingFactor);
    }
    private void HandleJump()
    {
        jumpTimer -= Time.deltaTime;
        if (wasJumpPressed && currentJumpCounter < consectiveJumpsAllowed - 1)
        {
            jumpTimer = jumpDuration;
            currentJumpCounter++;
        }
        if (characterController.isGrounded)
        {
            jumpTimer = jumpDuration;
            currentJumpCounter = 0;
        }
    }
    private void HandleRoll()
    {
        rollTimer -= Time.deltaTime;
        if (playerState == PlayerState.AIR_JUMP_FALL)
        {
            rollTimer = rollDuration;
        }
    }

    private void PlayAnimations()
    {
        if (playerState == PlayerState.ROLL)
        {
            playerAnimator.Play(rollHash);
        }

        else if (playerState == PlayerState.AIR_JUMP)
        {
            playerAnimator.SetFloat(yVelocityHash, playerDirection.y);
            playerAnimator.CrossFade(airJumpHash, 0.2f);
        }

        else if (playerState == PlayerState.JUMP)
        {
            playerAnimator.SetFloat(yVelocityHash, playerDirection.y);
            playerAnimator.Play(jumpHash);
        }

        else if (playerState == PlayerState.MOVE || playerState == PlayerState.IDLE)
        {
            playerAnimator.SetFloat(xVelocityHash, playerDirection.x);
        }
    }
}
