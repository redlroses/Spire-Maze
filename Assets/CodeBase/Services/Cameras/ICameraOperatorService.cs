using CodeBase.Logic.Cameras;
using UnityEngine;

namespace CodeBase.Services.Cameras
{
    public interface ICameraOperatorService : IService
    {
        void Focus(Transform followTarget);

        void RegisterCamera(CameraFollower cameraMovement);

        void FocusOnDefault();

        void SetAsDefault(Transform target);
    }
}