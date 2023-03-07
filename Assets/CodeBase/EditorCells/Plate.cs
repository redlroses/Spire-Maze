using UnityEngine;

namespace CodeBase.EditorCells
{
    public class Plate : CellData
    {
        public Plate(Texture2D texture) : base(texture)
        {
        }

        public override CellData Copy() =>
            new Plate(Texture);
    }
}