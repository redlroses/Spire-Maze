using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public class LiftDestinationMarker : MonoBehaviour
    {
        [SerializeField] private MovingDirection _direction;

        public MovingDirection Direction => _direction;
        public CellPosition Position { get; private set; }

        private void Awake()
        {
            Position = new CellPosition(transform.position.y, transform.rotation.eulerAngles.y.Clamp360());
        }
    }
}