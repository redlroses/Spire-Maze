using UnityEngine;

namespace CodeBase.EditorCells
{
    public class Portal : Plate
    {
        public Color32 Color;
        public int Key;

        public Portal(Texture2D texture, int key = 0, Color32 color = default)
            : base(texture)
        {
            Key = key;
            Color = color;
        }

        public override CellData Copy() =>
            new Portal(Texture, Key, Color);
    }
}