using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public class GearAnimation : MonoCache
    {
        [SerializeField] private int _rotationSpeed;

        protected override void Run()
        {
            transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
        }
        
        public void InvertRotationDirection()
        {
            _rotationSpeed *= -1;
        }
    }
}