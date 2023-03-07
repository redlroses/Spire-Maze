using UnityEngine;

namespace CodeBase.Data.Cell
{
    public class InitialPlate : Plate
    {
        public InitialPlate(Texture2D texture) : base(texture)
        {
        }

        public override CellData Copy2() =>
            new InitialPlate(Texture);
    }
}