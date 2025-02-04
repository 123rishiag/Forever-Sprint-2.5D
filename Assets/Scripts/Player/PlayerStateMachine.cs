using ServiceLocator.Utility;

namespace ServiceLocator.Player
{
    public enum PlayerState
    {
        IDLE,
        MOVE,
        JUMP,
        AIR_JUMP,
        FALL,
        BIG_FALL,
        DEAD_FALL,
        ROLL,
        SLIDE,
        DASH,
        CLIMB,
        KNOCK,
        GET_UP,
        DEAD
    }

    public class PlayerStateMachine : GenericStateMachine<PlayerController, PlayerState>
    {
        public PlayerStateMachine(PlayerController _owner) : base(_owner)
        {
            owner = _owner;
            CreateStates();
            SetOwner();
        }

        private void CreateStates()
        {
            States.Add(PlayerState.IDLE, new PlayerIdleState<PlayerController>(this));
            States.Add(PlayerState.MOVE, new PlayerMoveState<PlayerController>(this));
            States.Add(PlayerState.JUMP, new PlayerJumpState<PlayerController>(this));
            States.Add(PlayerState.AIR_JUMP, new PlayerAirJumpState<PlayerController>(this));
            States.Add(PlayerState.FALL, new PlayerFallState<PlayerController>(this));
            States.Add(PlayerState.BIG_FALL, new PlayerBigFallState<PlayerController>(this));
            States.Add(PlayerState.DEAD_FALL, new PlayerDeadFallState<PlayerController>(this));
            States.Add(PlayerState.ROLL, new PlayerRollState<PlayerController>(this));
            States.Add(PlayerState.SLIDE, new PlayerSlideState<PlayerController>(this));
            States.Add(PlayerState.DASH, new PlayerDashState<PlayerController>(this));
            States.Add(PlayerState.CLIMB, new PlayerClimbState<PlayerController>(this));
            States.Add(PlayerState.KNOCK, new PlayerKnockState<PlayerController>(this));
            States.Add(PlayerState.GET_UP, new PlayerGetUpState<PlayerController>(this));
            States.Add(PlayerState.DEAD, new PlayerDeadState<PlayerController>(this));
        }
    }
}
