using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerGetUpState<T> : IState<PlayerController, PlayerState>
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public PlayerGetUpState(PlayerStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter() { }
        public void Update()
        {
            if (Owner.GetView().GetUpFinished())
            {
                stateMachine.ChangeState(PlayerState.IDLE);
            }
        }
        public void FixedUpdate() { }
        public void OnStateExit()
        {
            // Setting Animator Position
            Owner.GetView().SetPosition(Owner.GetView().GetAnimator().bodyPosition);

            // Adding Offset
            Owner.GetView().SetPosition(Owner.GetTransform().position -
                new Vector3(0f, 1.5f, 0f));
        }
    }
}