using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class MovingMarker : CellData
    {
        public PlateMoveDirection Direction;
        public bool IsLiftHolder;

        public MovingMarker(Texture2D texture, PlateMoveDirection direction = PlateMoveDirection.Up, bool isLiftHolder = false) : base(texture)
        {
            IsLiftHolder = isLiftHolder;
            Direction = direction;
        }

        public override CellData Copy2() =>
            new MovingMarker(Texture, Direction, IsLiftHolder);
    }
}