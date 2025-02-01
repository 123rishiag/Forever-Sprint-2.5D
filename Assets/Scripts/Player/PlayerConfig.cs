using System;
using UnityEngine;

namespace ServiceLocator.Player
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "ScriptableObjects/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public PlayerService playerPrefab;
        public PlayerData playerData;
    }

    [Serializable]
    public class PlayerData
    {
        [Header("Health Settings")]
        public int maxHealth;

        [Header("Movement Settings")]
        public float moveSpeed;
        public float jumpForce;
        public float airJumpForce;
        public int airJumpAllowed;
        public float rollDuration;

        [Header("Slide Settings")]
        public float slideDuration;

        [Header("Dash Settings")]
        public float dashDuration;
        public float dashSpeedIncreaseFactor;
    }
}