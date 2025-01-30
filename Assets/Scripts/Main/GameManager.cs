using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private UIManager uiManager;

    private void Start()
    {
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (inputManager.IsEscapePressed)
        {
            PauseGame();
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        uiManager.mainMenuPanel.SetActive(false);
        soundManager.PlaySoundEffect(SoundType.GAME_PLAY);
    }
    public void PauseGame()
    {
        if (!uiManager.pauseMenuPanel.activeInHierarchy)
        {
            Time.timeScale = 0f;
            uiManager.pauseMenuPanel.SetActive(true);
            soundManager.PlaySoundEffect(SoundType.GAME_PAUSE);
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        uiManager.pauseMenuPanel.SetActive(false);
        soundManager.PlaySoundEffect(SoundType.GAME_PLAY);
    }
    public void RestartGame()
    {
        soundManager.PlaySoundEffect(SoundType.GAME_PLAY);
        SceneManager.LoadScene(0); // Reload 0th scene
    }
    public void MainMenu()
    {
        soundManager.PlaySoundEffect(SoundType.BUTTON_QUIT);
        SceneManager.LoadScene(0); // Reload 0th scene
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        uiManager.gameOverMenuPanel.SetActive(true);
        soundManager.PlaySoundEffect(SoundType.GAME_OVER);
    }
    public void QuitGame()
    {
        soundManager.PlaySoundEffect(SoundType.BUTTON_QUIT);
        Application.Quit();
    }

    public void MuteGame()
    {
        soundManager.MuteGame();
        uiManager.SetMuteButtonText(soundManager.IsMute);
        soundManager.PlaySoundEffect(SoundType.BUTTON_CLICK);
    }
}
