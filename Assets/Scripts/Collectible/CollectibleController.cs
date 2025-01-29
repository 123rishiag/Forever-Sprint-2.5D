using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.GetComponent<PlayerManager>() != null)
        {
            Destroy(gameObject);
        }
    }
}