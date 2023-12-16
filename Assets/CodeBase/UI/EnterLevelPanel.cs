using AYellowpaper;
using CodeBase.Tools;
using CodeBase.UI.Elements;
using I2.Loc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class EnterLevelPanel : MonoBehaviour
    {
        private readonly LocalizedString _levelTerm = "Level";

        [SerializeField] private Button _enterButton;
        [SerializeField] private InterfaceReference<IShowHide> _showHide;
        [SerializeField] private TextSetter _labelSetter;
        [SerializeField] private StarsView _starsView;

        public event UnityAction EnterClicked
        {
            add => _enterButton.onClick.AddListener(value);
            remove => _enterButton.onClick.RemoveListener(value);
        }

        public void Show(int starsCount, int levelId)
        {
            _starsView.EnableStars(starsCount);
            _labelSetter.SetText(string.Format(_levelTerm, levelId));
            _showHide.Value.Show();
            _enterButton.interactable = true;
        }

        public void Hide()
        {
            _enterButton.interactable = false;
            _showHide.Value.Hide();
        }
    }
}