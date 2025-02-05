using System;
using UnityEngine;

namespace ServiceLocator.Player
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public PlayerView playerPrefab;
        public PlayerData playerData;
    }

    [Serializable]
    public class PlayerData
    {
        [Header("Health Settings")]
        public int maxHealth;

        [Header("Movement Settings")]
        public float moveSpeed;

        [Header("Gravity Settings")]
        public float jumpForce;
        public float airJumpForce;
        public int airJumpAllowed;
        public float gravityForce;
        public float fallThreshold;
        public float bigFallThreshold;
        public float deadFallThreshold;

        [Header("Roll Settings")]
        public float rollDuration;
        public float rollSpeedDecreaseFactor;

        [Header("Slide Settings")]
        public float slideDuration;

        [Header("Dash Settings")]
        public float dashDuration;
        [Range(1, 2)] public float dashSpeedIncreaseFactor;
    }
}