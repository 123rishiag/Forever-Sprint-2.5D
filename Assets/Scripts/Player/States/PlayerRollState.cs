using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerRollState<T> : IState<PlayerController, PlayerState>
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public PlayerRollState(PlayerStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            // Setting Animator
            Owner.GetView().ResetProperty();
            Owner.GetView().GetAnimator().Play(PlayerView.rollHash);

            // Setting Elements
            Owner.SetVelocity(0f);
            Owner.RollTimer = Owner.GetModel().RollDuration;
        }
        public void Update()
        {
            Owner.RollTimer -= Time.deltaTime;
            if (Owner.RollTimer <= 0 && Owner.GetView().HasGroundAbove())
            {
                stateMachine.ChangeState(PlayerState.SLIDE);
            }
            else if (Owner.RollTimer <= 0)
            {
                stateMachine.ChangeState(PlayerState.IDLE);
            }
            else if (!Owner.GetView().IsGrounded())
            {
                stateMachine.ChangeState(PlayerState.FALL);
            }
        }
        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}