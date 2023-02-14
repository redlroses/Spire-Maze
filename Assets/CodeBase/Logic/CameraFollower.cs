using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic
{
    public class CameraFollower : MonoCache
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _cameraHolder;
        [SerializeField] private Vector3 _offsetRotetion;
        [SerializeField] private Vector3 _offsetPosition;

        protected override void LateRun()
        {
            _cameraHolder.rotation = Quaternion.LookRotation(new Vector3(-_target.position.x + _offsetRotetion.x, _offsetRotetion.y, -_target.position.z + _offsetRotetion.z));
            _cameraHolder.position = new Vector3(0, _target.position.y, 0);
            transform.localPosition = new Vector3(_offsetPosition.x, _offsetPosition.y, _offsetPosition.z);
        }
    }
}