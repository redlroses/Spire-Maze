using UnityEngine;

namespace CodeBase.EditorCells
{
    public class Savepoint : Plate
    {
        public Savepoint(Texture2D texture)
            : base(texture)
        {
        }

        public override CellData Copy() =>
            new Savepoint(Texture);
    }
}