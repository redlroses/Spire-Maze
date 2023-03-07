using System;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    public class DoorState : CellState
    {
        public bool IsOpen;

        public DoorState(int id, bool isOpen) : base(id)
        {
            IsOpen = isOpen;
        }
    }
}