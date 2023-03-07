using UnityEngine;

namespace CodeBase.EditorCells
{
    public class Wall : CellData
    {
        public Wall(Texture2D texture) : base(texture)
        {
        }

        public override CellData Copy() =>
            new Wall(Texture);
    }
}