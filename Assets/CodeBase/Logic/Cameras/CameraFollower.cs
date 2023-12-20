using System;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using NTC.Global.Cache;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.Logic.Cameras
{
    public class CameraFollower : MonoCache, IResolutionDependency
    {
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private Vector3 _offsetRotetion;
        [SerializeField] private Vector3 _offsetPosition;

        [SerializeField] private float _smoothing;
        [SerializeField] [Range(0, 1f)] private float _smoothPower = 0.5f;

        private string _currentOrientation;
        private Action _followAction = () => { };
        private Transform _followTarget;
        private IStaticDataService _staticData;
        private Vector3 _toPosition;

        private void Awake() =>
            enabled = true;

        public void Construct(IStaticDataService staticData)
        {
            _staticData = staticData;
            OnResolutionChanged();
        }

        public void OnResolutionChanged()
        {
            if (Application.isPlaying == false)
                return;

            if (_staticData == null)
                return;

            ScreenTypeConditions currentScreenConfiguration = ResolutionMonitor.CurrentScreenConfiguration;

            if (currentScreenConfiguration == null)
                return;

            string orientationName = currentScreenConfiguration.Name;

            if (orientationName == _currentOrientation)
                return;

            _currentOrientation = orientationName;
            ApplyConfig(_staticData.GetCameraConfigByOrientation(orientationName));
        }

        public void Follow(Transform target)
        {
            _followTarget = target;
            _followAction = FollowToTarget;
        }

        protected override void LateRun()
        {
            _followAction.Invoke();

#if UNITY_EDITOR
            ResolutionMonitor.Update();
            OnResolutionChanged();
#endif
        }

        private void FollowToTarget()
        {
            Vector3 targetPosition = _followTarget.position;

            Quaternion newRotation = Quaternion.LookRotation(
                new Vector3(
                    -targetPosition.x + _offsetRotetion.x,
                    _offsetRotetion.y,
                    -targetPosition.z + _offsetRotetion.z));

            Vector3 newPosition = new Vector3(0, targetPosition.y, 0);
            transform.localPosition = new Vector3(_offsetPosition.x, _offsetPosition.y, _offsetPosition.z);

            Vector3 position = _cameraHolder.position;
            Quaternion rotation = _cameraHolder.rotation;

            rotation = Quaternion.Lerp(
                rotation,
                newRotation,
                GetLerpFactor(rotation.eulerAngles, newRotation.eulerAngles));

            _cameraHolder.rotation = rotation;

            position = Vector3.Lerp(
                position,
                newPosition,
                GetLerpFactor(position, newPosition));

            _cameraHolder.position = position;
        }

        private float GetLerpFactor(Vector3 position, Vector3 newPosition) =>
            _smoothing * Time.deltaTime * Mathf.Pow(Vector3.Distance(position, newPosition), _smoothPower);

        private void ApplyConfig(CameraConfig config)
        {
            _offsetRotetion = config.OffsetRotation;
            _offsetPosition = config.OffsetPosition;
            Camera.main.fieldOfView = config.FieldOfView;
        }
    }
}