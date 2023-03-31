using CodeBase.Logic.Inventory;
using UnityEngine;

namespace CodeBase.UI
{
    public class InventoryView : MonoBehaviour
    {
        private IInventory _inventory;

        private void OnDestroy()
        {
            _inventory.Updated -= OnUpdatedInventory;
        }

        public void Construct(HeroInventory heroInventory)
        {
            _inventory = heroInventory.Inventory;
            _inventory.Updated += OnUpdatedInventory;
        }

        private void OnUpdatedInventory()
        {
            throw new System.NotImplementedException();
        }
    }
}
