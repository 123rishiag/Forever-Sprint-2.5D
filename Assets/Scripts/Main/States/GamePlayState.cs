using ServiceLocator.Sound;
using ServiceLocator.Utility;
using UnityEngine;

namespace ServiceLocator.Main
{
    public class GamePlayState<T> : IState<GameController, GameState>
    {
        public GameController Owner { get; set; }
        private GameStateMachine stateMachine;

        public GamePlayState(GameStateMachine _stateMachine) => stateMachine = _stateMachine;

        public void OnStateEnter()
        {
            Time.timeScale = 1f; // Resume the game
            Owner.GetSoundService().PlaySoundEffect(SoundType.GAME_PLAY);

            // Setting Camera
            var playerController = Owner.GetPlayerService().GetPlayerController();
            Owner.GetCameraService().SetMainCameraPosition(playerController.GetFollowTransform(),
                playerController.GetTransform());
            Owner.GetCameraService().SetMiniCameraParent(playerController.GetTransform());
        }
        public void Update()
        {
            Owner.GetInputService().Update();
            Owner.GetCameraService().Update();
            // No Sound Service Update
            // No UI Service Update
            // No Score Service Update
            Owner.GetPlayerService().Update();
            Owner.GetCollectibleService().Update();
            Owner.GetLevelService().Update();
            CheckGamePause();
            CheckGameOver();
        }
        public void FixedUpdate()
        {
            // No Input Service Fixed Update
            // No Camera Service Fixed Update
            // No Sound Service Fixed Update
            // No UI Service Fixed Update
            // No Score Service Fixed Update
            Owner.GetPlayerService().FixedUpdate();
            // No Collectible Service Fixed Update
            // No Level Service Fixed Update
        }
        public void OnStateExit()
        {
            Time.timeScale = 0f; // Stop the game
        }

        private void CheckGamePause()
        {
            if (Owner.GetInputService().IsEscapePressed)
            {
                stateMachine.ChangeState(GameState.GAME_PAUSE);
            }
        }
        private void CheckGameOver()
        {
            if (!Owner.GetPlayerService().GetPlayerController().IsAlive())
            {
                stateMachine.ChangeState(GameState.GAME_OVER);
            }
        }
    }
}