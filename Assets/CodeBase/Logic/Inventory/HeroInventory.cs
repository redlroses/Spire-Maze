using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Logic.Inventory
{
    public class HeroInventory : MonoBehaviour, ISavedProgress
    {
        public IInventory Inventory { get; private set; }

        public void LoadProgress(PlayerProgress progress)
        {
            Inventory = progress.HeroInventoryData.AsHeroInventory();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroInventoryData = Inventory.AsInventoryData();
        }
    }
}