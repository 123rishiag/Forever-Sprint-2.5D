namespace ServiceLocator.Player
{
    public class PlayerModel
    {
        public PlayerModel(PlayerData _playerData)
        {
            PlayerState = PlayerState.IDLE;
            MaxHealth = _playerData.maxHealth;
            MoveSpeed = _playerData.moveSpeed;
            JumpForce = _playerData.jumpForce;
            AirJumpForce = _playerData.airJumpForce;
            AirJumpAllowed = _playerData.airJumpAllowed;
            GravityForce = _playerData.gravityForce;
            FallThreshold = _playerData.fallThreshold;
            BigFallThreshold = _playerData.bigFallThreshold;
            DeadFallThreshold = _playerData.deadFallThreshold;
            RollDuration = _playerData.rollDuration;
            SlideDuration = _playerData.slideDuration;
            DashDuration = _playerData.dashDuration;
            DashSpeedIncreaseFactor = _playerData.dashSpeedIncreaseFactor;
        }

        public PlayerState PlayerState { get; set; }
        public int MaxHealth { get; private set; }
        public float MoveSpeed { get; private set; }
        public float JumpForce { get; private set; }
        public float AirJumpForce { get; private set; }
        public int AirJumpAllowed { get; private set; }
        public float GravityForce { get; private set; }
        public float FallThreshold { get; private set; }
        public float BigFallThreshold { get; private set; }
        public float DeadFallThreshold { get; private set; }
        public float RollDuration { get; private set; }
        public float SlideDuration { get; private set; }
        public float DashDuration { get; private set; }
        public float DashSpeedIncreaseFactor { get; private set; }
    }
}