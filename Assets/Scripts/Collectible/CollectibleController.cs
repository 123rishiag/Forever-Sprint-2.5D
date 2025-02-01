using ServiceLocator.Main;
using ServiceLocator.Player;
using ServiceLocator.Sound;
using UnityEngine;

namespace ServiceLocator.Collectible
{
    public class CollectibleController : MonoBehaviour
    {
        private GameService gameService;

        private void OnTriggerEnter(Collider _collider)
        {
            gameService = GameObject.Find("GameManager").GetComponent<GameService>();
            if (_collider.GetComponent<PlayerService>() != null)
            {
                gameService.GetGameController().GetScoreService().AddScore(10);
                Destroy(gameObject);
                gameService.GetGameController().GetSoundService().PlaySoundEffect(SoundType.COLLECTIBLE_PICKUP);
            }
        }
    }
}