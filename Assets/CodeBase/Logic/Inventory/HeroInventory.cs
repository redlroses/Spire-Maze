using System.Linq;
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
            Inventory = progress.WorldData.HeroInventoryData.AsHeroInventory();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (Inventory is null)
                return;

            progress.WorldData.HeroInventoryData = Inventory.AsInventoryData();
            progress.WorldData.LevelAccumulationData.Artifacts = Inventory
                .Where(inventoryCell => inventoryCell.Item.IsInteractive == false && inventoryCell.Item.IsExpendable == false)
                .Sum(inventoryCell => inventoryCell.Count);
        }
    }
}