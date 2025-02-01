using ServiceLocator.Main;
using ServiceLocator.Sound;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private GameService gameService;

    private void OnTriggerEnter(Collider _collider)
    {
        gameService = GameObject.Find("GameManager").GetComponent<GameService>();
        if (_collider.GetComponent<PlayerManager>() != null)
        {
            gameService.GetGameController().GetScoreService().AddScore(10);
            Destroy(gameObject);
            gameService.GetGameController().GetSoundService().PlaySoundEffect(SoundType.COLLECTIBLE_PICKUP);
        }
    }
}