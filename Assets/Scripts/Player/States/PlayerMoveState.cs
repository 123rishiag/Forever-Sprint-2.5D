using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public class PlayerMoveState<T> : IState<PlayerController, PlayerState>
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public PlayerMoveState(PlayerStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            // Setting Animator
            Owner.GetView().ResetProperty();
            Owner.GetView().GetAnimator().Play(PlayerView.moveHash);

            // Setting Collider Dimensions
            Owner.GetView().SetDefaultColliderDimensions();
        }
        public void Update()
        {
            if (Owner.InputService.WasJumpPressed)
            {
                stateMachine.ChangeState(PlayerState.JUMP);
            }
            else if (Owner.InputService.IsSlidePressed)
            {
                stateMachine.ChangeState(PlayerState.SLIDE);
            }
            else if (Owner.GetView().IsGrounded() && Owner.InputService.IsDashPressed)
            {
                stateMachine.ChangeState(PlayerState.DASH);
            }
            else if (!Owner.GetView().IsGrounded())
            {
                stateMachine.ChangeState(PlayerState.FALL);
            }
            else if (Owner.GetView().HasGroundRight())
            {
                stateMachine.ChangeState(PlayerState.IDLE);
            }
        }
        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}