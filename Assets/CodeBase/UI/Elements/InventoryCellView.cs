using System;
using CodeBase.Logic;
using CodeBase.Logic.Inventory;
using CodeBase.Logic.Items;
using CodeBase.StaticData.Storable;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    [RequireComponent(typeof(TimerOperator))]
    public class InventoryCellView : MonoBehaviour
    {
        private const int OnStartShowCount = 1;

        [SerializeField] private Button _button;
        [SerializeField] private Image _iconItem;
        [SerializeField] private TextSetter _textSetter;
        [SerializeField] private VisualTimer _visualTimer;
        [SerializeField] private TimerOperator _timer;

        private UnityAction _onClick = () => { };
        private IReadOnlyInventoryCell _cellItem;

        public event Action<StorableType> ItemUsed = _ => { };

        public StorableType ItemType { get; private set; }

        private void Start() =>
            _button.onClick.AddListener(_onClick);

        private void OnDestroy() =>
            _button.onClick.RemoveAllListeners();

        public void SetUp(IReadOnlyInventoryCell cell)
        {
            _cellItem = cell;
            IItem cellItem = cell.Item;
            ItemType = cellItem.ItemType;
            _iconItem.sprite = cellItem.Sprite;
            _button.interactable = cellItem.IsInteractive;
            _onClick += () => ItemUsed.Invoke(ItemType);
            SetCount(_cellItem);

            if (cellItem is IReloadable reloadable)
            {
                SetUpTimers(reloadable);
                _onClick += PlayVisualTimer;
                _onClick += PlayTimer;
                _onClick += () => _button.interactable = false;
            }
        }

        public void Update() =>
            SetCount(_cellItem);

        public void Remove() =>
            Destroy(gameObject);

        private void SetUpTimers(IReloadable reloadable)
        {
            _visualTimer.SetUp(reloadable.ReloadTime);
            _timer.SetUp(reloadable.ReloadTime, () => _button.interactable = true);
        }

        private void PlayVisualTimer() =>
            _visualTimer.StartReload();

        private void PlayTimer()
        {
            _timer.Restart();
            _timer.Play();
        }

        private void SetCount(IReadOnlyInventoryCell cell)
        {
            if (cell.Count <= OnStartShowCount)
            {
                _textSetter.gameObject.SetActive(false);
                return;
            }

            _textSetter.gameObject.SetActive(true);
            _textSetter.SetText(cell.Count);
        }
    }
}