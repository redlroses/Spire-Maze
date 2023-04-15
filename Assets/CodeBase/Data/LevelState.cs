﻿using System;
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
        public List<SavepointState> SavepointStates;
        public List<EnemyState> EnemyStates;
        public List<RockState> RockStates;
        public List<PortalState> PortalStates;

        public LevelState(string levelKey)
        {
            LevelKey = levelKey;
            DoorStates = new List<DoorState>();
            KeyStates = new List<KeyState>();
            SavepointStates = new List<SavepointState>();
            EnemyStates = new List<EnemyState>();
            RockStates = new List<RockState>();
            PortalStates = new List<PortalState>();
        }
    }
}