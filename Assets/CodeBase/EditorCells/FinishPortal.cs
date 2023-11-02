using UnityEngine;

namespace CodeBase.EditorCells
{
    public class FinishPortal : Plate
    {
        public FinishPortal(Texture2D texture) : base(texture) { }

        public override CellData Copy() =>
            new FinishPortal(Texture);
    }
}