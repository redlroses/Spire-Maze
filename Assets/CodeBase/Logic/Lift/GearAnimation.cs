using CodeBase.Logic.Lift.PlateMove;
using NTC.Global.Cache.Interfaces;
using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public class GearAnimation : IRunSystem
    {
        private int _rotationSpeed;
        private IPlateMover _mover;

        public void Construct(IPlateMover mover) =>
            _mover = mover;

        public void InvertRotationDirection() =>
            _rotationSpeed = -_rotationSpeed;

        public void OnRun()
        {
            transform.Rotate(0, _rotationSpeed * _mover.Speed * Time.deltaTime, 0);
        }
    }
}