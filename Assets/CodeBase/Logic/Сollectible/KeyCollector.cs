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
                [Colors.Red] = progress.HeroInventoryOld.RedKeys,
                [Colors.Green] = progress.HeroInventoryOld.GreenKeys,
                [Colors.Blue] = progress.HeroInventoryOld.BlueKeys,
                [Colors.Rgb] = progress.HeroInventoryOld.RgbKeys
            };
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroInventoryOld.RedKeys = _keys[Colors.Red];
            progress.HeroInventoryOld.GreenKeys = _keys[Colors.Green];
            progress.HeroInventoryOld.BlueKeys = _keys[Colors.Blue];
            progress.HeroInventoryOld.RgbKeys = _keys[Colors.Rgb];
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