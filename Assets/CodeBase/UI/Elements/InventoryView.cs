using System.Collections.Generic;
using System.Linq;
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
                return;

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
                BuildCellView(cell);
            }
        }

        private void BuildCellView(IReadOnlyInventoryCell cell)
        {
            InventoryCellView newCellView = CreateNewCellView();
            newCellView.SetUp(cell);
            newCellView.ItemUsed += OnItemUsed;
            _cellViews.Add(newCellView);
        }

        private InventoryCellView CreateNewCellView() =>
            _uiFactory.CreateCellView(_selfTransform).GetComponent<InventoryCellView>();

        private void RemoveCellView(InventoryCellView cellView)
        {
            cellView.ItemUsed -= OnItemUsed;
            cellView.Remove();
            _cellViews.Remove(cellView);
        }

        private void OnItemUsed(StorableType itemType) =>
            _inventory.TryUse(itemType);

        private void OnUpdatedInventory(IReadOnlyInventoryCell readOnlyInventoryCell)
        {
            InventoryCellView targetCellView = FirstOrDefaultCell(readOnlyInventoryCell);

            if (targetCellView is null)
            {
                BuildCellView(readOnlyInventoryCell);
                return;
            }

            if (readOnlyInventoryCell.Count <= 0)
            {
                RemoveCellView(targetCellView);
                return;
            }

            targetCellView.Update();
        }

        private InventoryCellView FirstOrDefaultCell(IReadOnlyInventoryCell readOnlyInventoryCell) =>
            _cellViews.FirstOrDefault(cell => cell.ItemType == readOnlyInventoryCell.Item.ItemType);
    }
}