using UnityEngine;

namespace CodeBase.EditorCells
{
    public class Air : CellData
    {
        public Air(Texture2D texture) : base(texture)
        {
        }

        public override CellData Copy() =>
            new Air(Texture);
    }
}