using System;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    public class KeyState : CellState
    {
        public bool IsTaken;

        public KeyState(int id, bool isTaken) : base(id)
        {
            IsTaken = isTaken;
        }
    }
}