using System.Collections.Generic;
using CodeBase.EditorCells;
using UnityEngine;

namespace CodeBase.Logic.Сollectible
{
    public class KeyCollector : ItemCollector<KeyCollectible>
    {
        private Dictionary<Colors, int> _keys;

        private void Awake()
        {
            _keys = new Dictionary<Colors, int>
            {
                [Colors.Red] = 0,
                [Colors.Green] = 0,
                [Colors.Blue] = 0,
                [Colors.Rgb] = 0,
            };
        }

        protected override void Collect(KeyCollectible item)
        {
            if (_keys.ContainsKey(item.Color))
            {
                _keys[item.Color]++;
            }
        }

        public bool TryUseKey(Colors keyColor)
        {
            if (HasKey(keyColor) == false)
            {
                return false;
            }

            _keys[keyColor]--;
            return true;
        }

        private bool HasKey(Colors color)
        {
            if (_keys.TryGetValue(color, out int count))
            {
                return count > 0;
            }

            return false;
        }
    }
}