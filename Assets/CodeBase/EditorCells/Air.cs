using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class Air : CellData
    {
        public Air(Texture2D texture) : base(texture)
        {
        }

        public override CellData Copy2() =>
            new Air(Texture);
    }
}