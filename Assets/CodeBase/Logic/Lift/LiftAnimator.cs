using System;
using CodeBase.EditorCells;
using CodeBase.Logic.Lift.PlateMove;
using CodeBase.Tools.Extension;
using NTC.Global.Cache;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public class LiftAnimator : MonoCache
    {
        [SerializeField] private Transform _gearLeft;
        [SerializeField] private Transform _gearRight;
        [SerializeField] private float _rotationSpeedFactor;

        private IPlateMover _mover;
        private bool _isGearLeftRotationClockwise;
        private bool _isGearRightRotationClockwise;

        public void Construct(PlateMoveDirection direction, IPlateMover mover)
        {
            _mover = mover;
            SetInitialGearsDirection(direction);
            this.Disable();
        }

        public void StartAnimation()
        {
            this.Enable();
            InvertDirection();
        }

        public void StopAnimation() =>
            this.Disable();

        protected override void Run()
        {
            AnimateRotation(_gearLeft, _isGearLeftRotationClockwise);
            AnimateRotation(_gearRight, _isGearRightRotationClockwise);
        }

        private void AnimateRotation(Transform gearLeft, bool isGearRotationClockwise) =>
            gearLeft.Rotate(Vector3.up,
                _mover.Velocity * _rotationSpeedFactor * isGearRotationClockwise.AsSign()
                * Time.deltaTime,
                Space.Self);

        private void InvertDirection()
        {
            _isGearLeftRotationClockwise = !_isGearLeftRotationClockwise;
            _isGearRightRotationClockwise = !_isGearRightRotationClockwise;
        }

        private void SetInitialGearsDirection(PlateMoveDirection direction)
        {
            switch (direction)
            {
                case PlateMoveDirection.Up:
                    SetupDirectionsForUpMovement();
                    break;
                case PlateMoveDirection.Left:
                    SetupDirectionsForLeftMovement();
                    break;
                case PlateMoveDirection.Right:
                    SetupDirectionsForRightMovement();
                    break;
                case PlateMoveDirection.Down:
                    SetupDirectionsForDownMovement();
                    break;
                case PlateMoveDirection.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private void SetupDirectionsForDownMovement()
        {
            _isGearLeftRotationClockwise = true;
            _isGearRightRotationClockwise = false;
        }

        private void SetupDirectionsForRightMovement()
        {
            _isGearLeftRotationClockwise = true;
            _isGearRightRotationClockwise = true;
        }

        private void SetupDirectionsForLeftMovement()
        {
            _isGearLeftRotationClockwise = false;
            _isGearRightRotationClockwise = false;
        }

        private void SetupDirectionsForUpMovement()
        {
            _isGearLeftRotationClockwise = false;
            _isGearRightRotationClockwise = true;
        }
    }
}