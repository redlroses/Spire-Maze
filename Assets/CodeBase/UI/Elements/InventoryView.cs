using CodeBase.Logic.Inventory;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class InventoryView : MonoBehaviour
    {
        private IInventory _inventory;

        private void OnDestroy()
        {
            if (_inventory == null)
            {
                return;
            }

            _inventory.Updated -= OnUpdatedInventory;
        }

        public void Construct(HeroInventory heroInventory)
        {
            _inventory = heroInventory.Inventory;
            _inventory.Updated += OnUpdatedInventory;
        }

        private void OnUpdatedInventory()
        {
            Debug.Log("Inventory updated");
        }
    }
}