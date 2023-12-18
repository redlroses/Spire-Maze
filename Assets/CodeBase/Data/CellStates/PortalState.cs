using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PortalState : CellState
    {
        public bool IsActivated;

        public PortalState(int id, bool isActivated)
            : base(id)
        {
            IsActivated = isActivated;
        }
    }
}