using ServiceLocator.Utility;

namespace ServiceLocator.Main
{
    public enum GameState
    {
        GAME_START,
        GAME_MENU,
        GAME_PLAY,
        GAME_PAUSE,
        GAME_RESTART,
        GAME_OVER
    }

    public class GameStateMachine : GenericStateMachine<GameController, GameState>
    {
        public GameStateMachine(GameController _owner) : base(_owner)
        {
            owner = _owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            States.Add(GameState.GAME_START, new GameStartState<GameController>(this));
            States.Add(GameState.GAME_MENU, new GameMenuState<GameController>(this));
            States.Add(GameState.GAME_PLAY, new GamePlayState<GameController>(this));
            States.Add(GameState.GAME_PAUSE, new GamePauseState<GameController>(this));
            States.Add(GameState.GAME_RESTART, new GameRestartState<GameController>(this));
            States.Add(GameState.GAME_OVER, new GameOverState<GameController>(this));
        }
    }
}
