using System;
using CodeBase.EditorCells;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public class LiftAnimator : MonoCache
    {
        [SerializeField] private GearAnimation _gearLeft;
        [SerializeField] private GearAnimation _gearRight;

        public void Construct(PlateMoveDirection direction)
        {
            SetStartDirectionGear(direction);
        }

        public void StartAnimation()
        {
            _gearLeft.enabled = true;
            _gearRight.enabled = true;
        }

        public void StopAnimation()
        {
            _gearLeft.enabled = false;
            _gearRight.enabled = false;
        }

        public void SetRotateDirection()
        {
            _gearLeft.InvertRotationDirection();
            _gearRight.InvertRotationDirection();
        }

        private void SetStartDirectionGear(PlateMoveDirection direction)
        {
            switch (direction)
            {
                case PlateMoveDirection.Up:
                    _gearLeft.InvertRotationDirection();
                    break;
                case PlateMoveDirection.Left:
                    _gearLeft.InvertRotationDirection();
                    _gearRight.InvertRotationDirection();
                    break;
            }
        }
    }
}