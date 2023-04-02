using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData.Storable;

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

        public void Add<TItem>(IStorable collectible) where TItem : IStorable
        {
            InventoryCell existingInventoryCell = _storage.First(inventoryCell => inventoryCell.Item is TItem);

            if (existingInventoryCell == null)
            {
                _storage.Add(new InventoryCell(collectible));
            }
            else
            {
                existingInventoryCell.IncreaseCount();
            }

            Updated?.Invoke();
        }

        public List<InventoryCell> ReadAll() => new List<InventoryCell>(_storage);


        public bool TryUse<TItem>(out TItem item) where TItem : IStorable
        {
            InventoryCell existingInventoryCell = _storage.First(inventoryCell => inventoryCell.Item is TItem);
            item = default;

            if (existingInventoryCell == null)
            {
                return false;
            }

            item = (TItem) existingInventoryCell.Item;
            RemoveItemFrom(existingInventoryCell);
            Updated?.Invoke();

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

        public void ClearUp() => _storage.Clear();

        public IEnumerator<IReadOnlyInventoryCell> GetEnumerator() => _storage.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}