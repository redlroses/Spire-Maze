using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class CellState
    {
        public int Id;

        public CellState(int id)
        {
            Id = id;
        }
    }
}