using System;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    public class SavepointState : CellState
    {
        public bool IsActive;
        
        public SavepointState(int id, bool isActive) : base(id)
        {
            IsActive = isActive;
        }
    }
}