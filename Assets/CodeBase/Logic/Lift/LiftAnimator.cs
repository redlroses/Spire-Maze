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
            SetInitialGearDirection(direction);
        }

        public void StartAnimation()
        {
            _gearLeft.enabled = true;
            _gearRight.enabled = true;

            InvertDirection();
        }

        public void StopAnimation()
        {
            _gearLeft.enabled = false;
            _gearRight.enabled = false;
        }

        private void InvertDirection()
        {
            _gearLeft.InvertRotationDirection();
            _gearRight.InvertRotationDirection();
        }

        private void SetInitialGearDirection(PlateMoveDirection direction)
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