using Cinemachine;
using UnityEngine;

namespace ServiceLocator.Vision
{
    public class CameraService
    {
        // Private Variables
        private CinemachineVirtualCamera virtualCamera;

        public CameraService(CinemachineVirtualCamera _virtualCamera)
        {
            // Setting Variables
            virtualCamera = _virtualCamera;
        }

        public void FollowPosition(Transform _followTransform, Transform _lookAtTransform)
        {
            virtualCamera.Follow = _followTransform;
            virtualCamera.LookAt = _lookAtTransform;
        }
    }
}