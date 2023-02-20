using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class Key : Plate
    {
        public Colors Color;

        public Key(Texture2D texture, Colors colorType = Colors.None) : base(texture)
        {
            Color = colorType;
        }
    }
}