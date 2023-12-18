using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RockState : CellState
    {
        public bool IsDestroyed;

        public RockState(int id, bool isDestroyed)
            : base(id)
        {
            IsDestroyed = isDestroyed;
        }
    }
}