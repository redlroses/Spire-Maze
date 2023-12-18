using UnityEngine;

namespace CodeBase.EditorCells
{
    public class ColoredCell : Plate
    {
        public Colors Color;

        public ColoredCell(Texture2D texture, Colors colorType = Colors.None)
            : base(texture)
        {
            Color = colorType;
        }

        public override CellData Copy() =>
            new ColoredCell(Texture, Color);
    }
}