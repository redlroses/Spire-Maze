using CodeBase.Logic.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class InventoryCellView : MonoBehaviour
    {
        [SerializeField] private Image _iconItem;
        [SerializeField] private TextMeshProUGUI _count;

        private IReadOnlyInventoryCell _cell;
        
        public void Add(IReadOnlyInventoryCell cell)
        {
            _cell = cell;
            Debug.Log(cell.Item);
            _iconItem.sprite = cell.Item.Sprite;
            _count.SetText(cell.Count.ToString());
        }

        public void Remove() => Destroy(gameObject);

        public void Refresh() => _count.SetText(_cell.Count.ToString());
    }
}