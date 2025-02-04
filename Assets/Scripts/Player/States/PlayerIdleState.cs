using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public class PlayerIdleState<T> : IState<PlayerController, PlayerState>
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public PlayerIdleState(PlayerStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.AirJumpCount = 0;
        }
        public void Update()
        {
            if (Owner.InputService.WasJumpPressed)
            {
                stateMachine.ChangeState(PlayerState.JUMP);
            }
            else if (Owner.InputService.IsSlidePressed && !Owner.GetView().CanSlide())
            {
                stateMachine.ChangeState(PlayerState.SLIDE);

            }
            else if (!Owner.GetView().HasGroundRight())
            {
                stateMachine.ChangeState(PlayerState.MOVE);
            }
            else
            {
                stateMachine.ChangeState(PlayerState.IDLE);
            }
        }
        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}