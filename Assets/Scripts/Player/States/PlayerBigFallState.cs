using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public class PlayerBigFallState<T> : IState<PlayerController, PlayerState>
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public PlayerBigFallState(PlayerStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            // Setting Animator
            Owner.GetView().ResetProperty();
            Owner.GetView().GetAnimator().Play(PlayerView.bigFallHash);
        }
        public void Update()
        {
            if (Owner.InputService.WasJumpPressed && Owner.AirJumpCount < Owner.GetModel().AirJumpAllowed)
            {
                stateMachine.ChangeState(PlayerState.AIR_JUMP);
            }
            else if (Owner.GetView().IsGrounded())
            {
                stateMachine.ChangeState(PlayerState.ROLL);
            }
            else if (Owner.GetView().CanClimb())
            {
                stateMachine.ChangeState(PlayerState.CLIMB);
            }
            else if (Owner.PlayerVelocity.y < -Owner.GetModel().DeadFallThreshold)
            {
                stateMachine.ChangeState(PlayerState.DEAD_FALL);
            }
        }
        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}