
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text healthText; // Text for displaying health
    [SerializeField] private TMP_Text scoreText; // Text for displaying score

    [Header("Pause Menu Elements")]
    [SerializeField] public GameObject pauseMenuPanel; // Pause Menu Panel
    [SerializeField] public Button pauseMenuResumeButton; // Button to resume game
    [SerializeField] public Button pauseMenuMainMenuButton; // Button to go to main menu

    [Header("Game Over Menu Elements")]
    [SerializeField] public GameObject gameOverMenuPanel; // Game Over Menu Panel
    [SerializeField] public Button gameOverMenuRestartButton; // Button to restart game
    [SerializeField] public Button gameOverMenuMainMenuButton; // Another button to go to main menu

    [Header("Main Menu Elements")]
    [SerializeField] public GameObject mainMenuPanel; // Main Menu Panel
    [SerializeField] public Button mainMenuPlayButton; // Button to start the game
    [SerializeField] public Button mainMenuQuitButton; // Button to quit the game
    [SerializeField] public Button mainMenuMuteButton; // Button to mute/unmute the game
    [SerializeField] public TMP_Text mainMenuMuteButtonText; // Text of mute button

    [Header("Animator")]
    [SerializeField] private Animator uiAnimator; // UI's Animator

    private void Start()
    {
        pauseMenuResumeButton.onClick.AddListener(gameManager.ResumeGame);
        pauseMenuMainMenuButton.onClick.AddListener(gameManager.MainMenu);

        gameOverMenuRestartButton.onClick.AddListener(gameManager.RestartGame);
        gameOverMenuMainMenuButton.onClick.AddListener(gameManager.MainMenu);

        mainMenuPlayButton.onClick.AddListener(gameManager.PlayGame);
        mainMenuQuitButton.onClick.AddListener(gameManager.QuitGame);
        mainMenuMuteButton.onClick.AddListener(gameManager.MuteGame);
    }

    private void ScoreBarAnimation()
    {
        uiAnimator.Rebind();
        uiAnimator.Update(0);
        uiAnimator.Play("ScoreBar");
    }

    private void HealthBarAnimation()
    {
        uiAnimator.Rebind();
        uiAnimator.Update(0);
        uiAnimator.Play("HealthBar");
    }

    // Setters
    public void UpdateHealthText(int _health)
    {
        healthText.text = "Health: " + _health;
        HealthBarAnimation();
    }
    public void UpdateScoreText(int _score)
    {
        scoreText.text = "Score: " + _score;
        ScoreBarAnimation();
    }
    public void SetMuteButtonText(bool _isMute)
    {
        mainMenuMuteButtonText.text = _isMute ? "Mute: Off" : "Mute: On"; // Toggle mute text
    }
}