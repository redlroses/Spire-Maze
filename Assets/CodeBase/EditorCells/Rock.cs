using UnityEngine;

namespace CodeBase.EditorCells
{
    public class Rock : Plate
    {
        public bool IsDirectionToRight;

        public Rock(Texture2D texture, bool isDirectionToRight = true) : base(texture)
        {
            IsDirectionToRight = isDirectionToRight;
        }

        public override CellData Copy() =>
            new Rock(Texture, IsDirectionToRight);
    }
}