﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic.Items;
using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Logic.Inventory
{
    public sealed class Inventory : IReadOnlyCollection<IReadOnlyInventoryCell>, IInventory
    {
        private readonly List<InventoryCell> _storage;

        public event Action<IReadOnlyInventoryCell> Updated = _ => { };

        public int Count => _storage.Count;

        public Inventory(List<InventoryCell> storage)
        {
            _storage = storage;
        }

        public void Cleanup() =>
            _storage.Clear();

        public void Add(IItem item)
        {
            if (TryGetInventoryCell(item.ItemType, out InventoryCell inventoryCell))
            {
                inventoryCell.Increase();
                Debug.Log($"Count item {inventoryCell.Count}");
            }
            else
            {
                inventoryCell = new InventoryCell(item);
                _storage.Add(inventoryCell);
            }

            Updated.Invoke(inventoryCell);
            Debug.Log($"SetUp {item.Name}");
        }

        public bool TryUse(StorableType storableType)
        {
            if (TryGetInventoryCell(storableType, out InventoryCell inventoryCell) == false)
                return false;

            if (inventoryCell.Item is IUsable usable)
                usable.Use();

            if (inventoryCell.Item is IExpendable)
                Expend(inventoryCell);

            return true;
        }

        private bool TryGetInventoryCell(StorableType byType, out InventoryCell inventoryCell)
        {
            inventoryCell = GetExistingInventoryCell(byType);
            return inventoryCell != null;
        }

        private InventoryCell GetExistingInventoryCell(StorableType storableType)
        {
            InventoryCell existingInventoryCell =
                _storage.FirstOrDefault(inventoryCell => inventoryCell.Item.ItemType == storableType);
            return existingInventoryCell;
        }

        private void Expend(InventoryCell existingInventoryCell)
        {
            existingInventoryCell.Decrease();

            if (existingInventoryCell.IsEmpty)
                _storage.Remove(existingInventoryCell);

            Updated.Invoke(existingInventoryCell);
        }

        public IEnumerator<IReadOnlyInventoryCell> GetEnumerator() => _storage.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}