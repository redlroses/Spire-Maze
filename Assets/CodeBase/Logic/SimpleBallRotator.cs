using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic
{
    public class SimpleBallRotator : MonoCache
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;

        protected override void Run()
        {
            _target.Rotate(_speed * Time.deltaTime, 0, 0, Space.Self);
        }
    }
}