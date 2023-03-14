using UnityEngine;

namespace CodeBase.EditorCells
{
    public class SpikeTrap : Plate
    {
        public SpikeTrap(Texture2D texture) : base(texture)
        {
        }

        public override CellData Copy() =>
            new SpikeTrap(Texture);
    }
}