using System;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    public class RockState : CellState
    {
        public bool IsDestroyed;
        
        public RockState(int id, bool isDestroyed) : base(id)
        {
            IsDestroyed = isDestroyed;
        }
    }
}