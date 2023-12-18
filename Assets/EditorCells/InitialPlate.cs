using UnityEngine;

namespace CodeBase.EditorCells
{
    public class InitialPlate : Plate
    {
        public InitialPlate(Texture2D texture)
            : base(texture)
        {
        }

        public override CellData Copy() =>
            new InitialPlate(Texture);
    }
}