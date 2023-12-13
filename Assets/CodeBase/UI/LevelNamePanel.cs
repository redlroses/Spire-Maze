using System.Collections.Generic;
using AYellowpaper;
using CodeBase.Tools;
using CodeBase.Tools.Extension;
using CodeBase.UI.Elements;
using Cysharp.Threading.Tasks;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.UI
{
    public class LevelNamePanel : MonoBehaviour
    {
        private readonly Dictionary<int, string> _levelTerms = new Dictionary<int, string>
        {
            [-1] = "TutorialLevelName",
            [0] = "HubLevelName",
        };

        [SerializeField] private InterfaceReference<IShowHide> _showHide;
        [SerializeField] private TextSetter _labelSetter;
        [SerializeField] private StarsView _starsView;
        [SerializeField] private float _showTime;

        public async UniTaskVoid Show(int starsCount, int levelId)
        {
            if (_levelTerms.TryGetValue(levelId, out string term))
            {
                _labelSetter.SetText(term.TranslateTerm());
                _starsView.gameObject.Disable();
            }
            else
            {
                string text = _labelSetter.ReadText();
                _labelSetter.SetText(string.Format(text, levelId));
                _starsView.EnableStars(starsCount);
            }

            _showHide.Value.Show();
            await UniTask.WaitForSeconds(_showTime, cancellationToken: gameObject.GetCancellationTokenOnDestroy());
            _showHide.Value.Hide();
        }
    }
}