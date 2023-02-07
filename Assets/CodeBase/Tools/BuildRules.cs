using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Level;

namespace CodeBase.Tools
{
    public class BuildRules
    {
        private readonly Dictionary<char, CellType> _rule;

        public BuildRules()
        {
            _rule = new Dictionary<char, CellType>
            {
                ['0'] = CellType.Air,
                ['1'] = CellType.Plate,
            };
        }

        public MapData Convert(string from)
        {
            UnityEngine.Debug.Log(from);
            CellType[] converted = new CellType[from.Length];

            for (int i = 0; i < from.Length; i++)
            {
                if (_rule.TryGetValue(from[i], out CellType cellType))
                {
                    converted[i] = cellType;
                }
                else
                {
                    throw new Exception($"Incorrect map data file: Level.txt. Unexpected symbol {from[i]}");
                }
            }

            return new MapData(converted);
        }
    }
}