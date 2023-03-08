using System;
using System.Collections.Generic;
using CodeBase.Data.CellStates;

namespace CodeBase.Data
{
    [Serializable]
    public class LevelState
    {
        public string LevelKey;
        public List<DoorState> DoorStates;
        public List<KeyState> KeyStates;

        public LevelState(string levelKey)
        {
            LevelKey = levelKey;
            DoorStates = new List<DoorState>();
            KeyStates = new List<KeyState>();
        }
    }
}