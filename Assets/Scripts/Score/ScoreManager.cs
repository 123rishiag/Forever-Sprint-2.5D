using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score;

    private void Start()
    {
        score = 0;
    }

    public void AddScore(int _score)
    {
        score += _score;
        Debug.Log(score);
    }
}
