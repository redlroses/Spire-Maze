using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.EditorCells;
using CodeBase.Services.PersistentProgress;

namespace CodeBase.Logic.Сollectible
{
    public class KeyCollector : ItemCollector<KeyCollectible>, ISavedProgress
    {
        private Dictionary<Colors, int> _keys;

        public void LoadProgress(PlayerProgress progress)
        {
            _keys = new Dictionary<Colors, int>
            {
                [Colors.Red] = progress.HeroInventory.RedKeys,
                [Colors.Green] = progress.HeroInventory.GreenKeys,
                [Colors.Blue] = progress.HeroInventory.BlueKeys,
                [Colors.Rgb] = progress.HeroInventory.RgbKeys
            };
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroInventory.RedKeys = _keys[Colors.Red];
            progress.HeroInventory.GreenKeys = _keys[Colors.Green];
            progress.HeroInventory.BlueKeys = _keys[Colors.Blue];
            progress.HeroInventory.RgbKeys = _keys[Colors.Rgb];
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