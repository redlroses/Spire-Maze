using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic.Item;
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
            if (TryGetExistingInventoryCell(item.ItemType, out InventoryCell existingInventoryCell) == false)
            {
                _storage.Add(new InventoryCell(item));
            }
            else
            {
                existingInventoryCell.IncreaseCount();
                Debug.Log($"Count item {existingInventoryCell.Count}");
            }

            Updated?.Invoke();
            Debug.Log($"SetUp {item.Name}");
        }

        public bool TryUse(StorableType storableType)
        {
            if (TryGetExistingInventoryCell(storableType, out InventoryCell existingInventoryCell) == false)
                return false;

            if (!(existingInventoryCell.Item is IUsable usable))
                return false;

            UseItem(usable, existingInventoryCell);
            return true;
        }

        public bool TrySpend(StorableType storableType)
        {
            if (TryGetExistingInventoryCell(storableType, out InventoryCell existingInventoryCell) == false)
                return false;

            RemoveItemFrom(existingInventoryCell);
            return true;
        }

        private bool TryGetExistingInventoryCell(StorableType storableType, out InventoryCell existingInventoryCell)
        {
            existingInventoryCell = GetExistingInventoryCell(storableType);
            return GetExistingInventoryCell(storableType) != null;
        }

        private InventoryCell GetExistingInventoryCell(StorableType storableType)
        {
            InventoryCell existingInventoryCell =
                _storage.FirstOrDefault(inventoryCell => inventoryCell.Item.ItemType == storableType);
            return existingInventoryCell;
        }

        private void UseItem(IUsable usable, InventoryCell existingInventoryCell)
        {
            usable.Use();
            RemoveItemFrom(existingInventoryCell);
        }

        private void RemoveItemFrom(InventoryCell existingInventoryCell)
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