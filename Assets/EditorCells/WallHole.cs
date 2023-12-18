using UnityEngine;

namespace CodeBase.EditorCells
{
    public class WallHole : CellData
    {
        public WallHole(Texture2D texture)
            : base(texture)
        {
        }

        public override CellData Copy() =>
            new WallHole(Texture);
    }
}