using ServiceLocator.Utility;

namespace ServiceLocator.Main
{
    public class GameMenuState<T> : IState<GameController, GameState>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GameMenuState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetUIService().GetUIController().EnableMainMenuPanel(true); // Show Main Menu
        }
        public void Update() { }
        public void FixedUpdate() { }
        public void OnStateExit()
        {
            Owner.GetUIService().GetUIController().EnableMainMenuPanel(false); // Hide Main Menu
        }
    }
}