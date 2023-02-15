using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public class LiftDestinationMarker : MonoBehaviour
    {
        public MovingDirection Direction { get; private set; }
        public CellPosition Position { get; private set; }

        public void Construct(CellPosition cellPosition, MovingDirection direction)
        {
            Position = cellPosition;
            Direction = direction;
        }
    }
}