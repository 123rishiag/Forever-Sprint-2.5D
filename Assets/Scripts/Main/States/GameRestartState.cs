using ServiceLocator.Utility;

namespace ServiceLocator.Main
{
    public class GameRestartState<T> : IState<GameController, GameState>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GameRestartState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.Reset();
            stateMachine.ChangeState(GameState.GAME_PLAY);
        }
        public void Update() { }
        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}