using System.Collections.Generic;
using AYellowpaper;
using CodeBase.Infrastructure;
using CodeBase.Tools;
using CodeBase.UI.Elements;
using Cysharp.Threading.Tasks;
using I2.Loc;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.UI
{
    public class LevelNamePanel : MonoBehaviour
    {
        private readonly LocalizedString _levelName = "Level";
        private readonly Dictionary<int, LocalizedString> _levelTerms = new Dictionary<int, LocalizedString>
        {
            [LevelNames.LearningLevelId] = new LocalizedString("TutorialLevelName"),
            [LevelNames.LobbyId] = new LocalizedString("HubLevelName"),
        };

        [SerializeField] private InterfaceReference<IShowHide> _showHide;
        [SerializeField] private TextSetter _labelSetter;
        [SerializeField] private StarsView _starsView;
        [SerializeField] private float _showTime;

        public async UniTaskVoid Show(int starsCount, int levelId)
        {
            if (_levelTerms.TryGetValue(levelId, out LocalizedString localizedString))
            {
                _labelSetter.SetText(localizedString);
                _starsView.gameObject.Disable();
            }
            else
            {
                _labelSetter.SetText(string.Format(_levelName, levelId));
                _starsView.EnableStars(starsCount);
            }

            _showHide.Value.Show();
            await UniTask.WaitForSeconds(_showTime, cancellationToken: gameObject.GetCancellationTokenOnDestroy());
            _showHide.Value.Hide(() => Destroy(gameObject));
        }
    }
}