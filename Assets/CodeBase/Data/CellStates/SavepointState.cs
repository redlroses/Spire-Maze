using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SavepointState : CellState
    {
        public bool IsActive;

        public SavepointState(int id, bool isActive)
            : base(id)
        {
            IsActive = isActive;
        }
    }
}