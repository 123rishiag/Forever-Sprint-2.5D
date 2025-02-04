using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerDashState<T> : IState<PlayerController, PlayerState>
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public PlayerDashState(PlayerStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            // Setting Animator
            Owner.GetView().ResetProperty();
            Owner.GetView().SetDashFactorHash(Owner.GetModel().DashSpeedIncreaseFactor);
            Owner.GetView().GetAnimator().Play(PlayerView.dashHash);

            // Setting Collider Dimensions
            Owner.GetView().SetDefaultColliderDimensions();

            // Setting Elements
            Owner.CurrentSpeed *= Owner.GetModel().DashSpeedIncreaseFactor;
        }
        public void Update()
        {
            // Restarting Dash Timer
            if (!Owner.GetView().HasGroundRight() && Owner.InputService.IsDashPressed && Owner.DashTimer <= 0)
            {
                Owner.DashTimer = Owner.GetModel().DashDuration; // Reset dash timer
            }

            Owner.DashTimer -= Time.deltaTime;
            if (Owner.InputService.WasJumpPressed && !Owner.GetView().HasGroundAbove())
            {
                stateMachine.ChangeState(PlayerState.JUMP);
            }
            else if (Owner.InputService.IsSlidePressed)
            {
                stateMachine.ChangeState(PlayerState.SLIDE);
            }
            else if (!Owner.GetView().IsGrounded())
            {
                stateMachine.ChangeState(PlayerState.FALL);
            }
            else if (Owner.GetView().HasGroundRight())
            {
                Owner.DecreaseHealth();
            }
            else if (Owner.DashTimer <= 0)
            {
                Owner.DashTimer = Owner.GetModel().DashDuration;
                stateMachine.ChangeState(PlayerState.IDLE);
            }
        }
        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}