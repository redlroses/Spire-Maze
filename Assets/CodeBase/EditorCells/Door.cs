using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class Door : ColoredCell
    {
        public Door(Texture2D texture, Colors colorType = Colors.None) : base(texture, colorType)
        {
        }

        public override CellData Copy2() =>
            new Door(Texture, Color);
    }
}