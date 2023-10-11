using System;
using AYellowpaper;
using CodeBase.Tools;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class EnterLevelPanel : MonoBehaviour
    {
        [SerializeField] private Button _enterButton;
        [SerializeField] private InterfaceReference<IShowHide> _showHide;
        [SerializeField] private StarsView _starsView;

        public event Action EnterClick
        {
            add => _enterButton.onClick.AddListener(value.Invoke);
            remove => _enterButton.onClick.RemoveAllListeners();
        }

        public void Show(int starsCount)
        {
            _starsView.EnableStars(starsCount);
            _showHide.Value.Show();
        }

        public void Hide()
        {
            _showHide.Value.Hide();
        }
    }
}