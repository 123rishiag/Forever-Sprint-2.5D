using Cinemachine;
using ServiceLocator.Controls;
using UnityEngine;

namespace ServiceLocator.Vision
{
    public class CameraService
    {
        // Private Variables
        private CinemachineVirtualCamera mainCamera;
        private Camera miniCamera;

        // Private Services
        private InputService inputService;

        public CameraService(CinemachineVirtualCamera _mainCamera, Camera _miniCamera)
        {
            // Setting Variables
            mainCamera = _mainCamera;
            miniCamera = _miniCamera;
        }

        public void Init(InputService _inputService)
        {
            // Setting Services
            inputService = _inputService;
        }

        public void Update() => ToggleMiniCamera();

        // Setters
        public void SetMainCameraPosition(Transform _followTransform, Transform _lookAtTransform)
        {
            mainCamera.Follow = _followTransform;
            mainCamera.LookAt = _lookAtTransform;
        }
        public void SetMiniCameraParent(Transform _parentTransform) =>
            miniCamera.transform.SetParent(_parentTransform.transform, false);

        public void ToggleMiniCamera()
        {
            if (inputService.WasToggleMiniCameraPressed)
            {
                GameObject miniCameraGameObject = miniCamera.transform.gameObject;
                if (miniCameraGameObject.activeInHierarchy) miniCameraGameObject.SetActive(false);
                else miniCameraGameObject.SetActive(true);
            }

        }
    }
}