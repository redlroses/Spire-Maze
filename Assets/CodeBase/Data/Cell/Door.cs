using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class Door : Plate
    {
        public Colors Color;

        public Door(Texture2D texture, Colors colorType = Colors.None) : base(texture)
        {
            Color = colorType;
        }
    }
}