using ServiceLocator.Level;
using ServiceLocator.Player;
using UnityEngine;

namespace ServiceLocator.Collectible
{
    public class CollectibleView : MonoBehaviour
    {
        [Header("Collectible Settings")]
        [SerializeField] private MeshRenderer collectibleMeshRenderer;

        // Private Variables
        private CollectibleController collectibleController;

        public void Init(CollectibleController _collectibleController)
        {
            // Setting Variables
            collectibleController = _collectibleController;
        }

        // Setters
        public void SetProperty(CollectibleProperty _collectibleProperty)
        {
            // Setting Texture
            Texture newTexture = _collectibleProperty.collectibleTexture;
            collectibleMeshRenderer.material.SetTexture("_MainTex", newTexture);
        }
        public void SetPosition(Vector3 _position) => transform.position = _position;
        public void ShowView() => gameObject.SetActive(true);
        public void HideView() => gameObject.SetActive(false);

        private void OnTriggerEnter(Collider _collider)
        {
            if (_collider.GetComponent<PlayerView>() != null)
            {
                collectibleController.AddScore();
            }
            else if (_collider.GetComponent<LevelView>() != null)
            {
                HideView();
            }
        }
    }
}