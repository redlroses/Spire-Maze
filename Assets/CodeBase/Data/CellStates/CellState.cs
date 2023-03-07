using System;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    public class CellState
    {
        public int Id;

        public CellState(int id)
        {
            Id = id;
        }
    }
}