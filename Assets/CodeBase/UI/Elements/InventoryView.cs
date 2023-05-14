using System.Collections.Generic;
using CodeBase.Logic.Inventory;
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
            _cellViews = new List<InventoryCellView>(_inventory.Count);

            foreach (IReadOnlyInventoryCell cell in _inventory)
            {
                CreateCellView(cell);
            }
        }

        private void CreateCellView(IReadOnlyInventoryCell cell)
        {
            var newCellView = Instantiate(_prefabCellView, transform);
            newCellView.Add(cell);
            _cellViews.Add(newCellView);
        }

        private void RemoveCellView(int index)
        {
                _cellViews[index].Remove();
                _cellViews[index] = null;
        }

        private void OnUpdatedInventory()
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory.GetEnumerator().MoveNext() == false)
                {
                    break;
                }
                
                if (i < _cellViews.Count)
                {
                    _cellViews[i].Refresh();
                    continue;
                }
            
                IReadOnlyInventoryCell test = _inventory.GetEnumerator().Current;
                Debug.Log($"{test} GetEnumerator");
                CreateCellView(test);
            }

            for (int i = _inventory.Count; i < Inventory.MaxCountItems; i++)
            {
                RemoveCellView(i);
            }

            Debug.Log("Inventory updated");
        }
    }
}