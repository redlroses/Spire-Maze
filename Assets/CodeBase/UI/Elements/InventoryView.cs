using System.Collections.Generic;
using CodeBase.Logic.Inventory;
using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private InventoryCellView _prefabCellView;

        private IInventory _inventory;
        private List<InventoryCellView> _cellViews;

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
            InitializeCell();
        }

        private void InitializeCell()
        {
            _cellViews = new List<InventoryCellView>();

            foreach (IReadOnlyInventoryCell cell in _inventory)
            {
                var cellView = CreateCellView(cell);
                cellView.SetUp(cell);
            }
        }

        private InventoryCellView CreateCellView(IReadOnlyInventoryCell cell)
        {
            var newCellView = Instantiate(_prefabCellView, transform);
            newCellView.SetUp(cell);
            newCellView.ItemUsed += OnItemUsed;
            _cellViews.Add(newCellView);
            Debug.Log("create");
            return newCellView;
        }

        private void RemoveCellView(int index)
        {
            _cellViews[index].ItemUsed -= OnItemUsed;
            _cellViews[index].Remove();
            _cellViews.RemoveAt(index);
        }

        private void OnItemUsed(StorableType itemType)
        {
            _inventory.TryUse(itemType, out StorableStaticData data);
        }

        private void OnUpdatedInventory()
        {
            int cellViewsCount = _cellViews.Count;
            using IEnumerator<IReadOnlyInventoryCell> enumerator = _inventory.GetEnumerator();

            for (int i = 0; i < _inventory.Count; i++)
            {
                enumerator.MoveNext();
                IReadOnlyInventoryCell cell = enumerator.Current;

                if (i < cellViewsCount)
                {
                    _cellViews[i].SetUp(cell);
                    continue;
                }

                CreateCellView(cell);
            }

            for (int i = _inventory.Count; i < cellViewsCount; i++)
            {
                RemoveCellView(i);
            }
        }
    }
}