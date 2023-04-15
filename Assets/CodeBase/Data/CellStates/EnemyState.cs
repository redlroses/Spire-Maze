using System;

namespace CodeBase.Data.CellStates
{
    [Serializable]
    public class EnemyState : CellState
    {
        public bool IsDied;
        
        public EnemyState(int id, bool isDied) : base(id)
        {
            IsDied = isDied;
        }
    }
}