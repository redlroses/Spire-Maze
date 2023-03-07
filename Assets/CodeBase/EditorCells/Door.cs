using UnityEngine;

namespace CodeBase.EditorCells
{
    public class Door : ColoredCell
    {
        public Door(Texture2D texture, Colors colorType = Colors.None) : base(texture, colorType)
        {
        }

        public override CellData Copy() =>
            new Door(Texture, Color);
    }
}