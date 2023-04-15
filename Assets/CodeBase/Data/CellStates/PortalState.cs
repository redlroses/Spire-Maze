using System;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    public class PortalState : CellState
    {
        public bool IsActivated;
        
        public PortalState(int id, bool isActivated) : base(id)
        {
            IsActivated = isActivated;
        }
    }
}