using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class KeyState : CellState
    {
        public bool IsTaken;

        public KeyState(int id, bool isTaken)
            : base(id)
        {
            IsTaken = isTaken;
        }
    }
}