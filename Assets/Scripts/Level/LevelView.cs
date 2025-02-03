using UnityEngine;

namespace ServiceLocator.Level
{
    public class LevelView : MonoBehaviour
    {
        [Header("Level Settings")]
        [SerializeField] private MeshRenderer levelMashRenderer;
        [SerializeField] private Transform startPointTransform;
        [SerializeField] private Transform endPointTransform;

        // Setters
        public void SetProperty(LevelProperty _levelProperty)
        {
            // Setting Texture
            Texture newTexture = _levelProperty.levelTexture;
            levelMashRenderer.material.SetTexture("_MainTex", newTexture);

            // Setting Transform
            transform.position = _levelProperty.levelPosition;
            transform.rotation = _levelProperty.levelRotation;
            transform.localScale = _levelProperty.levelScale;
        }
        public void SetPosition(Vector3 _position)
        {
            Vector3 newPosition = new Vector3(_position.x - startPointTransform.position.x,
                _position.y,
                transform.position.z);
            transform.position = newPosition;
        }
        public void ShowView() => gameObject.SetActive(true);
        public void HideView() => gameObject.SetActive(false);

        // Getters
        public Transform GetEndPointTransform() => endPointTransform;
        public Bounds GetLevelBounds()
        {
            float topY = transform.position.y + (transform.localScale.y / 2);
            float leftX = transform.position.x - (transform.localScale.x / 2);
            float rightX = transform.position.x + (transform.localScale.x / 2);
            return new Bounds(new Vector3(transform.position.x, topY, transform.position.z),
                              new Vector3(rightX - leftX, 0f, 0f));
        }

    }
}