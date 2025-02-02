using UnityEngine;

namespace ServiceLocator.Level
{
    public class LevelView : MonoBehaviour
    {
        [Header("Level Settings")]
        [SerializeField] private Transform startPointTransform;
        [SerializeField] private Transform endPointTransform;

        // Setters
        public void SetPosition(Vector3 _position)
        {
            _position.x -= startPointTransform.position.x;
            transform.position = _position;
        }

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