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
            Owner.SoundService.PlaySoundEffect(SoundType.PLAYER_KNOCK);
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