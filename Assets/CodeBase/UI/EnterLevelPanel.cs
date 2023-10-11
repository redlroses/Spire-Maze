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
        [SerializeField] private TextSetter _labelSetter;
        [SerializeField] private StarsView _starsView;

        public event Action EnterClick
        {
            add => _enterButton.onClick.AddListener(value.Invoke);
            remove => _enterButton.onClick.RemoveAllListeners();
        }

        public void Show(int starsCount, int levelId)
        {
            _starsView.EnableStars(starsCount);
            string text = _labelSetter.ReadText();
            _labelSetter.SetText(String.Format(text, levelId));
            _showHide.Value.Show();
        }

        public void Hide()
        {
            _showHide.Value.Hide();
        }
    }
}