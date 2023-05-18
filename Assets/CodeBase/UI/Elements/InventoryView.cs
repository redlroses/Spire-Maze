using System.Collections.Generic;
using CodeBase.Logic.Inventory;
using CodeBase.StaticData.Storable;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class InventoryView : MonoBehaviour
    {
        private List<InventoryCellView> _cellViews;
        private IInventory _inventory;
        private IUIFactory _uiFactory;
        private Transform _selfTransform;

        private void OnDestroy()
        {
            if (_inventory == null)
            {
                return;
            }

            _inventory.Updated -= OnUpdatedInventory;
        }

        public void Construct(IUIFactory uiFactory, HeroInventory heroInventory)
        {
            _selfTransform = transform;
            _uiFactory = uiFactory;
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
            InventoryCellView newCellView = CreateNewCellView();
            newCellView.SetUp(cell);
            newCellView.ItemUsed += OnItemUsed;
            _cellViews.Add(newCellView);
            return newCellView;
        }

        private InventoryCellView CreateNewCellView() =>
            _uiFactory.CreateCellView(_selfTransform).GetComponent<InventoryCellView>();

        private void RemoveCellView(int index)
        {
            _cellViews[index].ItemUsed -= OnItemUsed;
            _cellViews[index].Remove();
            _cellViews.RemoveAt(index);
        }

        private void OnItemUsed(StorableType itemType)
        {
            _inventory.TryUse(itemType);
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