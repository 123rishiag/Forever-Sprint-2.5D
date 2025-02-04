using ServiceLocator.Sound;
using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerSlideState<T> : IState<PlayerController, PlayerState>
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public PlayerSlideState(PlayerStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.SoundService.PlaySoundEffect(SoundType.PLAYER_SLIDE);
        }
        public void Update()
        {
            // Restarting Slide Timer
            if (!Owner.GetView().HasGroundAbove() && Owner.InputService.IsSlidePressed && Owner.SlideTimer <= 0)
            {
                Owner.SlideTimer = Owner.GetModel().SlideDuration; // Reset slide timer
            }

            Owner.SlideTimer -= Time.deltaTime;
            if (Owner.InputService.WasJumpPressed && !Owner.GetView().HasGroundAbove())
            {
                stateMachine.ChangeState(PlayerState.JUMP);
            }
            else if (!Owner.GetView().IsGrounded())
            {
                stateMachine.ChangeState(PlayerState.FALL);
            }
            else if (Owner.SlideTimer <= 0 && !Owner.GetView().HasGroundAbove())
            {
                stateMachine.ChangeState(PlayerState.IDLE);
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