using System;
using CodeBase.Logic.Inventory;
using CodeBase.StaticData.Storable;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class InventoryCellView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _iconItem;
        [SerializeField] private TextSetter _textSetter;

        private StorableType _itemType;
        public event Action<StorableType> ItemUsed;

        private void Start()
        {
            _button.onClick.AddListener(() =>
                ItemUsed?.Invoke(_itemType));
        }

        private void OnDestroy() =>
            _button.onClick.RemoveAllListeners();

        public void SetUp(IReadOnlyInventoryCell cell)
        {
            _itemType = cell.Item.ItemType;
            _iconItem.sprite = cell.Item.Sprite;
            _textSetter.SetText(cell.Count);
        }

        public void Remove() => Destroy(gameObject);
    }
}