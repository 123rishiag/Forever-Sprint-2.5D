using ServiceLocator.Sound;
using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public class PlayerKnockState<T> : IState<PlayerController, PlayerState>
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public PlayerKnockState(PlayerStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            // Setting Animator
            Owner.GetView().ResetProperty();
            Owner.GetView().GetAnimator().Play(PlayerView.knockHash);

            // Setting Collider Dimensions
            Owner.GetView().SetDefaultColliderDimensions();

            // Setting Elements
            Owner.EventService.PlaySoundEffectEvent.Invoke(SoundType.PLAYER_KNOCK);
        }
        public void Update()
        {
            if (Owner.GetView().KnockFinished())
            {
                stateMachine.ChangeState(PlayerState.GET_UP);
            }
        }
        public void FixedUpdate() { }
        public void OnStateExit()
        {
            // Setting Animator Position
            Owner.GetView().SetPosition(Owner.GetView().GetAnimator().bodyPosition);
        }
    }
}