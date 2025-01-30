using UnityEngine;

public class CollectibleController : MonoBehaviour
{

    private ScoreManager scoreManager;
    private void OnTriggerEnter(Collider _collider)
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        if (_collider.GetComponent<PlayerManager>() != null)
        {
            scoreManager.AddScore(10);
            Destroy(gameObject);
        }
    }
}