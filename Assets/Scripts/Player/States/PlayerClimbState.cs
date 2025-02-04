using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class PlayerClimbState<T> : IState<PlayerController, PlayerState>
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public PlayerClimbState(PlayerStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.SetVelocity(0f);

            // Adding Offset
            Owner.GetView().SetPosition(Owner.GetTransform().position + new Vector3(0.30f, 0f, 0f));
            //playerView.transform.position += new Vector3(0.30f, -0.14f, 0f);
        }
        public void Update()
        {
            if (Owner.GetView().ClimbFinished())
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
                new Vector3(0f, Owner.GetView().GetCharacterController().height, 0f));
        }
    }
}