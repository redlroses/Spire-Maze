using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DoorState : CellState
    {
        public bool IsOpen;

        public DoorState(int id, bool isOpen)
            : base(id)
        {
            IsOpen = isOpen;
        }
    }
}