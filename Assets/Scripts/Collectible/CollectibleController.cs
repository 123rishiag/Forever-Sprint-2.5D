using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private ScoreManager scoreManager;
    private SoundManager soundManager;

    private void OnTriggerEnter(Collider _collider)
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        if (_collider.GetComponent<PlayerManager>() != null)
        {
            scoreManager.AddScore(10);
            Destroy(gameObject);
            soundManager.PlaySoundEffect(SoundType.COLLECTIBLE_PICKUP);
        }
    }
}