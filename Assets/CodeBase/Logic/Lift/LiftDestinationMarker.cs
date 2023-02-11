using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public class LiftDestinationMarker : MonoBehaviour
    {
        [SerializeField] private MovingDirection _direction;
        [SerializeField] private float _angle;

        public MovingDirection Direction => _direction;
        public CellPosition Position { get; private set; }

        private void Awake()
        {
            Position = new CellPosition(transform.position.y, _angle);
        }
    }
}