using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private InputManager inputManager;

    private bool isMute;

    private void Start()
    {
        Time.timeScale = 0f;
        isMute = false;
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
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        uiManager.pauseMenuPanel.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        uiManager.pauseMenuPanel.SetActive(false);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0); // Reload 0th scene
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0); // Reload 0th scene
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        uiManager.gameOverMenuPanel.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void MuteGame()
    {
        isMute = !isMute;
        uiManager.SetMuteButtonText(isMute);
    }
}
