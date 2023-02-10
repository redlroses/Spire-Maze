using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic
{
    public class CameraFollower : MonoCache
    {
        [SerializeField] private Transform _target;

        protected override void LateRun()
        {
            transform.LookAt(_target);
        }
    }
}