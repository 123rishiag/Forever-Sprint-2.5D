using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public class PlayerDeadFallState<T> : IState<PlayerController, PlayerState>
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public PlayerDeadFallState(PlayerStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            // Setting Animator
            Owner.GetView().ResetProperty();
            Owner.GetView().GetAnimator().Play(PlayerView.deadFallHash);

            // Setting Collider Dimensions
            Owner.GetView().SetDefaultColliderDimensions();
        }
        public void Update()
        {
            if (Owner.InputService.WasJumpPressed && Owner.AirJumpCount < Owner.GetModel().AirJumpAllowed)
            {
                stateMachine.ChangeState(PlayerState.AIR_JUMP);
            }
            else if (Owner.GetView().IsGrounded())
            {
                Owner.DecreaseHealth();
            }
            else if (Owner.GetView().CanClimb())
            {
                stateMachine.ChangeState(PlayerState.CLIMB);
            }
        }

        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}