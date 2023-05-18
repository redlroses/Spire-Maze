using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic.Items;
using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Inventory
{
    public class Inventory : IReadOnlyCollection<IReadOnlyInventoryCell>, IInventory
    {
        private readonly List<InventoryCell> _storage;

        public event Action Updated;

        public int Count => _storage.Count;

        public Inventory(List<InventoryCell> storage)
        {
            _storage = storage;
        }

        public void Cleanup() =>
            _storage.Clear();

        public List<InventoryCell> ReadAll() =>
            new List<InventoryCell>(_storage);

        public void Add(IItem item)
        {
            if (TryGetInventoryCell(item.ItemType, out InventoryCell inventoryCell) == false)
            {
                _storage.Add(new InventoryCell(item));
            }
            else
            {
                inventoryCell.IncreaseCount();
                Debug.Log($"Count item {inventoryCell.Count}");
            }

            Updated?.Invoke();
            Debug.Log($"SetUp {item.Name}");
        }

        public bool TryUse(StorableType storableType)
        {
            if (TryGetInventoryCell(storableType, out InventoryCell inventoryCell) == false)
                return false;

            if (!(inventoryCell.Item is IUsable usable))
                return false;

            usable.Use();

            if (inventoryCell.Item is IExpendable)
                Expend(inventoryCell);

            return true;
        }

        private bool TryGetInventoryCell(StorableType byType, out InventoryCell inventoryCell)
        {
            inventoryCell = GetExistingInventoryCell(byType);
            return GetExistingInventoryCell(byType) != null;
        }

        private InventoryCell GetExistingInventoryCell(StorableType storableType)
        {
            InventoryCell existingInventoryCell =
                _storage.FirstOrDefault(inventoryCell => inventoryCell.Item.ItemType == storableType);
            return existingInventoryCell;
        }

        private void Expend(InventoryCell existingInventoryCell)
        {
            existingInventoryCell.DecreaseCount();

            if (existingInventoryCell.IsEmpty)
            {
                _storage.Remove(existingInventoryCell);
            }

            Updated?.Invoke();
        }

        public IEnumerator<IReadOnlyInventoryCell> GetEnumerator() => _storage.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}