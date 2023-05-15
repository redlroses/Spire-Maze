using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void Add(StorableStaticData item)
        {
            InventoryCell existingInventoryCell = _storage.FirstOrDefault(inventoryCell => inventoryCell.Item.ItemType == item.ItemType);

            if (existingInventoryCell == null)
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

        public List<InventoryCell> ReadAll() => new List<InventoryCell>(_storage);


        public bool TryUse(StorableType storableType, out StorableStaticData item)
        {
            InventoryCell existingInventoryCell = _storage.FirstOrDefault(inventoryCell => inventoryCell.Item.ItemType == storableType);
            item = default;

            if (existingInventoryCell == null)
            {
                return false;
            }

            item = existingInventoryCell.Item;
            RemoveItemFrom(existingInventoryCell);
            Updated?.Invoke();

            Debug.Log($"Use {item.Name}");

            return true;
        }

        private void RemoveItemFrom(InventoryCell existingInventoryCell)
        {
            existingInventoryCell.DecreaseCount();

            if (existingInventoryCell.IsEmpty)
            {
                _storage.Remove(existingInventoryCell);
            }
        }

        public void Cleanup() => _storage.Clear();

        public IEnumerator<IReadOnlyInventoryCell> GetEnumerator() => _storage.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}