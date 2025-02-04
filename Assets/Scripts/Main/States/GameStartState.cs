using ServiceLocator.Utility;

namespace ServiceLocator.Main
{
    public class GameStartState<T> : IState<GameController, GameState>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GameStartState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.Reset();
            stateMachine.ChangeState(GameState.GAME_MENU);
        }
        public void Update() { }
        public void FixedUpdate() { }
        public void OnStateExit() { }
    }
}