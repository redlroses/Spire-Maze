using System;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic
{
    public class CameraFollower : MonoCache
    {
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private Vector3 _offsetRotetion;
        [SerializeField] private Vector3 _offsetPosition;

        [SerializeField] private float _smoothing;
        [SerializeField] [Min(0f)] [MaxValue(1f)] private float _power = 0.5f;

        private Transform _followTarget;
        private Vector3 _toPosition;
        private Action _moveAction;

        private void Awake()
        {
            enabled = true;
        }

        protected override void FixedRun() =>
            _moveAction.Invoke();

        public void Follow(Transform target)
        {
            _followTarget = target;
            _moveAction = FollowToTarget;
        }

        private void FollowToTarget()
        {
            Vector3 targetPosition = _followTarget.position;

            Quaternion newRotation = Quaternion.LookRotation(new Vector3(-targetPosition.x + _offsetRotetion.x, _offsetRotetion.y, -targetPosition.z + _offsetRotetion.z));
            Vector3 newPosition = new Vector3(0, targetPosition.y, 0);
            transform.localPosition = new Vector3(_offsetPosition.x, _offsetPosition.y, _offsetPosition.z);

            Vector3 position = _cameraHolder.position;

            _cameraHolder.rotation = Quaternion.Lerp(
                _cameraHolder.rotation,
                newRotation,
                _smoothing * Time.deltaTime / Mathf.Pow(Vector3.Distance(_cameraHolder.rotation.eulerAngles, newRotation.eulerAngles), 1f - _power));

            position = Vector3.Lerp(
                position,
                newPosition,
                _smoothing * Time.deltaTime / Mathf.Pow(Vector3.Distance(position, newPosition), 1f - _power));
            _cameraHolder.position = position;
        }
    }
}