using ServiceLocator.Sound;
using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public class PlayerAirJumpState<T> : IState<PlayerController, PlayerState>
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public PlayerAirJumpState(PlayerStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            // Setting Animator
            Owner.GetView().ResetProperty();
            Owner.GetView().GetAnimator().Play(PlayerView.airJumpHash);

            // Setting Collider Dimensions
            Owner.GetView().SetDefaultColliderDimensions();

            // Setting Elements
            Owner.CurrentSpeed = Owner.DefaultSpeed;
            ++Owner.AirJumpCount;
            Owner.SetVelocityY(Owner.GetModel().AirJumpForce);
            Owner.EventService.PlaySoundEffectEvent.Invoke(SoundType.PLAYER_AIR_JUMP);
        }
        public void Update()
        {
            if (Owner.InputService.WasJumpPressed && Owner.AirJumpCount < Owner.GetModel().AirJumpAllowed)
            {
                stateMachine.ChangeState(PlayerState.AIR_JUMP);
            }
            else if (Owner.GetView().IsGrounded())
            {
                stateMachine.ChangeState(PlayerState.IDLE);
            }
            else if (Owner.GetView().CanClimb())
            {
                stateMachine.ChangeState(PlayerState.CLIMB);
            }
            else if (Owner.PlayerVelocity.y < -Owner.GetModel().FallThreshold)
            {
                stateMachine.ChangeState(PlayerState.FALL);
            }
        }
        public void FixedUpdate() => Owner.Move();
        public void OnStateExit() { }
    }
}