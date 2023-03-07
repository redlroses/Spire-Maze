using System;
using System.Collections.Generic;
using CodeBase.Data.CellStates;

namespace CodeBase.Data
{
    [Serializable]
    public class LevelState
    {
        public string LevelName;
        public List<DoorState> DoorStates;

        public LevelState()
        {
            DoorStates = new List<DoorState>();
        }
    }
}