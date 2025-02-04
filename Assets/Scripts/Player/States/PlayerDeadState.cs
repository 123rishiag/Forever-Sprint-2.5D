using ServiceLocator.Sound;
using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public class PlayerDeadState<T> : IState<PlayerController, PlayerState>
    {
        public PlayerController Owner { get; set; }
        private PlayerStateMachine stateMachine;

        public PlayerDeadState(PlayerStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            // Setting Animator
            Owner.GetView().ResetProperty();
            Owner.GetView().GetAnimator().Play(PlayerView.deadHash);

            // Setting Collider Dimensions
            Owner.GetView().SetDefaultColliderDimensions();

            // Setting Elements
            Owner.SoundService.PlaySoundEffect(SoundType.PLAYER_DEAD);
        }
        public void Update() { }
        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}