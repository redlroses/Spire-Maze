using UnityEngine;

namespace CodeBase.EditorCells
{
    public class FireTrap : Plate
    {
        public FireTrap(Texture2D texture) : base(texture)
        {
        }

        public override CellData Copy() =>
            new FireTrap(Texture);
    }
}