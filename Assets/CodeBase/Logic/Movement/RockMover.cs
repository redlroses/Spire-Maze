using UnityEngine;

namespace CodeBase.Logic.Movement
{
    public class RockMover : Mover
    {
        private Transform _selfTransform;

        public MoveDirection Direction { get; private set; }

        private void Awake()
        {
            _selfTransform = transform;
        }

        public void SetMoveDirection(Vector3 activatorPosition)
        {
            Direction = CalculateDirection(activatorPosition);
            Move(Direction);
        }
        private MoveDirection CalculateDirection(Vector3 activatorPosition)
        {
            float dotDirection = Vector3.Dot(_selfTransform.forward.normalized, (activatorPosition - _selfTransform.position).normalized);
            MoveDirection direction = dotDirection < 0 ? MoveDirection.Left : MoveDirection.Right;

            return direction;
        }
    }
}