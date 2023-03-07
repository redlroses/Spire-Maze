using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class Wall : CellData
    {
        public Wall(Texture2D texture) : base(texture)
        {
        }

        public override CellData Copy2() =>
            new Wall(Texture);
    }
}