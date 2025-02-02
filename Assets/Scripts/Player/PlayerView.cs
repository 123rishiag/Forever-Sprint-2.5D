using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerView : MonoBehaviour
    {
        [Header("Inspector Attachments")]
        [SerializeField] private bool allowGizmos;
        [SerializeField] private Transform followTransform;
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController characterController;

        [Header("Ground Detection")]
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float groundAboveDetectionDistance;
        [SerializeField] private float groundRightDetectionDistance;

        [Header("Collider Settings")]
        [SerializeField] private Vector3 defaultCenter;
        [SerializeField] private float defaultHeight;
        [SerializeField] private float rollingHeightMultiplier;
        [SerializeField] private float slidingHeightMultiplier;

        // Private Variables
        private PlayerController playerController;

        // Animator Hashes
        private readonly int dashFactorHash = Animator.StringToHash("Dash Factor");
        private readonly int idleHash = Animator.StringToHash("Idle");
        private readonly int moveHash = Animator.StringToHash("Move");
        private readonly int jumpHash = Animator.StringToHash("Jump");
        private readonly int airJumpHash = Animator.StringToHash("Air Jump");
        private readonly int fallHash = Animator.StringToHash("Fall");
        private readonly int bigFallHash = Animator.StringToHash("Fall");
        private readonly int deadFallHash = Animator.StringToHash("Fall");
        private readonly int rollHash = Animator.StringToHash("Roll");
        private readonly int slideHash = Animator.StringToHash("Slide");
        private readonly int dashHash = Animator.StringToHash("Move");
        private readonly int climbHash = Animator.StringToHash("Climb");
        private readonly int knockHash = Animator.StringToHash("Knock");
        private readonly int getUpHash = Animator.StringToHash("Get Up");
        private readonly int deadHash = Animator.StringToHash("Dead");

        public void Init(PlayerController _playerController)
        {
            // Setting Variables
            playerController = _playerController;
        }

        #region AnimationHandling
        public void PlayAnimation()
        {
            animator.SetFloat(dashFactorHash, 0);
            switch (playerController.GetModel().PlayerState)
            {
                case PlayerState.IDLE:
                    animator.Play(idleHash);
                    break;
                case PlayerState.MOVE:
                    animator.Play(moveHash);
                    break;
                case PlayerState.JUMP:
                    animator.Play(jumpHash);
                    break;
                case PlayerState.AIR_JUMP:
                    animator.Play(airJumpHash);
                    break;
                case PlayerState.FALL:
                    animator.Play(fallHash);
                    break;
                case PlayerState.BIG_FALL:
                    animator.Play(bigFallHash);
                    break;
                case PlayerState.DEAD_FALL:
                    animator.Play(deadFallHash);
                    break;
                case PlayerState.ROLL:
                    animator.Play(rollHash);
                    break;
                case PlayerState.SLIDE:
                    animator.Play(slideHash);
                    break;
                case PlayerState.DASH:
                    animator.SetFloat(dashFactorHash, playerController.GetModel().DashSpeedIncreaseFactor);
                    animator.Play(dashHash);
                    break;
                case PlayerState.CLIMB:
                    animator.Play(climbHash);
                    break;
                case PlayerState.KNOCK:
                    animator.Play(knockHash);
                    break;
                case PlayerState.GET_UP:
                    animator.Play(getUpHash);
                    break;
                case PlayerState.DEAD:
                    animator.Play(deadHash);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region CollisonHandling
        public void UpdateColliderDimensions()
        {
            switch (playerController.GetModel().PlayerState)
            {
                case PlayerState.ROLL:
                    characterController.height = defaultHeight * rollingHeightMultiplier;
                    characterController.center = new Vector3(defaultCenter.x, defaultHeight * rollingHeightMultiplier / 2,
                        defaultCenter.z);
                    break;
                case PlayerState.SLIDE:
                    characterController.height = defaultHeight * slidingHeightMultiplier;
                    characterController.center = new Vector3(defaultCenter.x, defaultHeight * slidingHeightMultiplier / 2,
                        defaultCenter.z);
                    break;
                default:
                    characterController.height = defaultHeight;
                    characterController.center = defaultCenter;
                    break;
            }
        }
        #endregion

        #region Getters
        public bool ClimbFinished()
        {
            return animator.GetCurrentAnimatorStateInfo(0).shortNameHash == climbHash &&
                   animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
        public bool KnockFinished()
        {
            return animator.GetCurrentAnimatorStateInfo(0).shortNameHash == knockHash &&
                   animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
        public bool GetUpFinished()
        {
            return animator.GetCurrentAnimatorStateInfo(0).shortNameHash == getUpHash &&
                   animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
        public bool DeadFinished()
        {
            return animator.GetCurrentAnimatorStateInfo(0).shortNameHash == deadHash &&
                   animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
        public bool CanSlide()
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
        public bool CanClimb()
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
        public bool HasGroundRight()
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
        public bool HasGroundAbove()
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
        public bool IsGrounded()
        {
            return characterController.isGrounded;
        }
        public Transform GetFollowTransform() => followTransform;
        public Animator GetAnimator() => animator;
        public CharacterController GetCharacterController() => characterController;
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