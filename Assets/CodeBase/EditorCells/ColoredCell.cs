using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class ColoredCell : Plate
    {
        public Colors Color;

        public ColoredCell(Texture2D texture, Colors colorType = Colors.None) : base(texture)
        {
            Color = colorType;
        }

        public override CellData Copy2() =>
            new ColoredCell(Texture, Color);
    }
}