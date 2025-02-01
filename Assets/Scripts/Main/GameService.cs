using ServiceLocator.Player;
using ServiceLocator.Sound;
using ServiceLocator.UI;
using UnityEngine;

namespace ServiceLocator.Main
{
    public class GameService : MonoBehaviour
    {
        // Inspector Elements
        [Header("Sound Elements")]
        [SerializeField] public SoundConfig soundConfig;
        [SerializeField] public AudioSource bgmSource;
        [SerializeField] public AudioSource sfxSource;

        [Header("UI Elements")]
        [SerializeField] public UIService uiCanvas;

        [Header("Game Elements")]
        [SerializeField] public PlayerConfig playerConfig;

        // Private Variables
        private GameController gameController;

        private void Start()
        {
            gameController = new GameController(this);
        }

        private void Update()
        {
            gameController.Update();
        }

        private void OnDestroy()
        {
            gameController.Destroy();
        }

        // Getters
        public GameController GetGameController() => gameController;
    }
}
