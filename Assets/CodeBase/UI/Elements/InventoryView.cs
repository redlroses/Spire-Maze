using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic.Inventory;
using CodeBase.Services.Input;
using CodeBase.StaticData.Storable;
using CodeBase.UI.Services.Factory;
using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.UI.Elements
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private BetterToggle _inventoryShowToggle;

        private List<InventoryCellView> _cellViews;
        private InputController _inputController;
        private IInventory _inventory;
        private Transform _selfTransform;
        private IUIFactory _uiFactory;

        private void OnDestroy()
        {
            if (_inventory == null)
                return;

            _inventory.Updated -= OnUpdatedInventory;
            _inputController.Player.OpenInventory.started -= OnOpenInventory;
        }

        public void Construct(IUIFactory uiFactory, HeroInventory heroInventory)
        {
            _selfTransform = transform;
            _uiFactory = uiFactory;
            _inventory = heroInventory.Inventory;
            _inventory.Updated += OnUpdatedInventory;
            InitializeCell();
            _inputController = new InputController();
            _inputController.Player.Enable();
            _inputController.Player.OpenInventory.started += OnOpenInventory;
        }

        private void InitializeCell()
        {
            _cellViews = new List<InventoryCellView>();

            foreach (IReadOnlyInventoryCell cell in _inventory)
                BuildCellView(cell);
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

        private void OnItemUsed(StorableType itemType)
        {
            if (_inventory.TryUse(itemType))
                _inventoryShowToggle.isOn = false;
        }

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

        private void OnOpenInventory(InputAction.CallbackContext context) =>
            _inventoryShowToggle.EmulateClick();
    }
}