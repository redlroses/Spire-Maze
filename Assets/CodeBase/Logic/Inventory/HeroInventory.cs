using System.Linq;
using CodeBase.Data;
using CodeBase.Logic.Items;
using CodeBase.Services.PersistentProgress;
using CodeBase.Tools.Extension;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Logic.Inventory
{
    public class HeroInventory : MonoBehaviour, ISavedProgress
    {
        public IInventory Inventory { get; private set; }

        private void Start()
        {
            Debug.Log("Press \"X\" for look into inventory");

            InputAction input = new InputAction("Press X", InputActionType.Button, "<Keyboard>/x");
            input.started += context =>
            {
                foreach (IReadOnlyInventoryCell cell in Inventory)
                {
                    Debug.Log($"Item:  {cell.Item.Name}, type: {cell.Item.ItemType}, count: {cell.Count}");
                }
            };

            input.Enable();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            Debug.Log("LoadProgress");
            Inventory = progress.WorldData.HeroInventoryData.AsHeroInventory();

            foreach (IReadOnlyInventoryCell cell in Inventory)
            {
                Debug.Log(cell.Item.Name);
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (Inventory is null)
                return;

            progress.WorldData.HeroInventoryData = Inventory.AsInventoryData();
            progress.WorldData.LevelAccumulationData.Artifacts = Inventory
                .Where(inventoryCell => inventoryCell.Item is IUsable == false)
                .Sum(inventoryCell => inventoryCell.Count);

            Debug.Log("Load progress inventory");
            foreach (var cell in progress.WorldData.HeroInventoryData.ItemDatas)
            {
                Debug.Log(cell.StorableType);
            }
        }
    }
}