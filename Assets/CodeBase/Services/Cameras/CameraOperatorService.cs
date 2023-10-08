using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Services.Cameras
{
    public class CameraOperatorService : ICameraOperatorService
    {
        private CameraFollower _activeCamera;
        private Transform _defaultFollowTarget;

        public void FocusOnDefault() =>
            Focus(_defaultFollowTarget);

        public void Focus(Transform followTarget) =>
            _activeCamera.Follow(followTarget);

        public void SetAsDefault(Transform target) =>
            _defaultFollowTarget = target;

        public void RegisterCamera(CameraFollower cameraMovement) =>
            _activeCamera = cameraMovement;
    }
}