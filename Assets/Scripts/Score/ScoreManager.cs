using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    private int score;

    private void Start()
    {
        score = 0;
    }

    public void AddScore(int _score)
    {
        score += _score;
        uiManager.UpdateScoreText(score);
    }
}
