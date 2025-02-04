using ServiceLocator.Sound;
using ServiceLocator.Utility;

namespace ServiceLocator.Main
{
    public class GamePauseState<T> : IState<GameController, GameState>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GamePauseState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Owner.GetUIService().GetUIController().EnablePauseMenuPanel(true); // Show Pause Menu
            Owner.GetSoundService().PlaySoundEffect(SoundType.GAME_PAUSE);
        }
        public void Update() { }
        public void FixedUpdate() { }
        public void OnStateExit()
        {
            Owner.GetUIService().GetUIController().EnablePauseMenuPanel(false); // Hide Pause Menu
        }
    }
}