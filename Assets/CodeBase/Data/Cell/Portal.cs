using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class Portal : Plate
    {
        public int Key;
        public Color32 Color;

        public Portal(Texture2D texture, int key = 0, Color32 color = default) : base(texture)
        {
            Key = key;
            Color = color;
        }

        public override CellData Copy2() =>
            new Portal(Texture, Key, Color);
    }
}