using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using NaughtyAttributes;
using NTC.Global.Cache;
using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Logic.Cameras
{
    public class CameraFollower : MonoCache, IResolutionDependency
    {
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private Vector3 _offsetRotetion;
        [SerializeField] private Vector3 _offsetPosition;

        [SerializeField] private float _smoothing;
        [SerializeField] [Range(0f, 1f)] private float _lerpPower = 0.5f;

        private string _currentOrientation;
        private Transform _followTarget;
        private Vector3 _toPosition;
        private Action _followAction = () => { };
        private IStaticDataService _staticData;

        private void Awake()
        {
            enabled = true;
        }

        public void Construct(IStaticDataService staticData)
        {
            _staticData = staticData;
            OnResolutionChanged();
        }

        public void OnResolutionChanged()
        {
            if (Application.isPlaying == false)
            {
                return;
            }

            if (_staticData == null)
            {
                return;
            }

            ScreenTypeConditions currentScreenConfiguration = ResolutionMonitor.CurrentScreenConfiguration;

            if (currentScreenConfiguration == null)
            {
                return;
            }

            string orientationName = currentScreenConfiguration.Name;

            if (orientationName == _currentOrientation)
            {
                return;
            }

            _currentOrientation = orientationName;
            ApplyConfig(_staticData.GetCameraConfigByOrientation(orientationName));
        }

        protected override void LateRun()
        {
            _followAction.Invoke();

#if UNITY_EDITOR
            ResolutionMonitor.Update();
            OnResolutionChanged();
#endif
        }

        public void Follow(Transform target)
        {
            _followTarget = target;
            _followAction = FollowToTarget;
        }

        private void FollowToTarget()
        {
            Vector3 targetPosition = _followTarget.position;

            Quaternion newRotation = Quaternion.LookRotation(new Vector3(-targetPosition.x + _offsetRotetion.x, _offsetRotetion.y, -targetPosition.z + _offsetRotetion.z));
            Vector3 newPosition = new Vector3(0, targetPosition.y, 0);
            transform.localPosition = new Vector3(_offsetPosition.x, _offsetPosition.y, _offsetPosition.z);

            Vector3 position = _cameraHolder.position;
            Quaternion rotation = _cameraHolder.rotation;

            rotation = Quaternion.Lerp(
                rotation,
                newRotation,
                _smoothing * Time.smoothDeltaTime / Mathf.Pow(Vector3.Distance(rotation.eulerAngles, newRotation.eulerAngles), 1f - _lerpPower));
            _cameraHolder.rotation = rotation;

            position = Vector3.Lerp(
                position,
                newPosition,
                _smoothing * Time.smoothDeltaTime / Mathf.Pow(Vector3.Distance(position, newPosition), 1f - _lerpPower));
            _cameraHolder.position = position;
        }

        private void ApplyConfig(CameraConfig config)
        {
            _offsetRotetion = config.OffsetRotation;
            _offsetPosition = config.OffsetPosition;
            Camera.main.fieldOfView = config.FieldOfView;
        }
    }
}