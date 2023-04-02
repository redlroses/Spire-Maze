using CodeBase.Data;
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
            InputAction input = new InputAction("Press X", InputActionType.Button, "<Keyboard>/x");
            input.started += context =>
            {
                Debug.Log("Items:");
                foreach (IReadOnlyInventoryCell cell in Inventory)
                {
                    Debug.Log($"Item:  {cell.Item.Name}, type: {cell.Item.ItemType}");
                }
            };

            input.Enable();
        }

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