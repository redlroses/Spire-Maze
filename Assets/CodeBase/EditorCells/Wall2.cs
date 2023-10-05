using UnityEngine;

namespace CodeBase.EditorCells
{
    public class Wall2 : CellData
    {
        public Wall2(Texture2D texture) : base(texture)
        {
        }

        public override CellData Copy() =>
            new Wall2(Texture);
    }
}