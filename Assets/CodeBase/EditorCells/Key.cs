using UnityEngine;

namespace CodeBase.EditorCells
{
    public class Key : ColoredCell
    {
        public Key(Texture2D texture, Colors colorType = Colors.None)
            : base(texture, colorType)
        {
        }

        public override CellData Copy() =>
            new Key(Texture, Color);
    }
}