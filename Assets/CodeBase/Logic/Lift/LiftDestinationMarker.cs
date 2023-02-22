using UnityEngine;

namespace CodeBase.Logic.Lift
{
    public class LiftDestinationMarker : MonoBehaviour
    {
        public CellPosition Position { get; private set; }

        public void Construct(CellPosition cellPosition)
        {
            Position = cellPosition;
        }
    }
}