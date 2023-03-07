using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class Plate : CellData
    {
        public Plate(Texture2D texture) : base(texture)
        {
        }

        public override CellData Copy2() =>
            new Plate(Texture);
    }
}