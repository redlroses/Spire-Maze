using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.Tools.Extension;
using CodeBase.Tutorial;
using CodeBase.UI.Elements;
using CodeBase.UI.Elements.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class TutorialWindow : WindowBase
    {
        [Header("Panel Settings")]
        [SerializeField] private Image _image;
        [SerializeField] private TextSetter _textSetter;

        [Space] [Header("Buttons")]
        [SerializeField] private SimpleButton _buttonNext;
        [SerializeField] private SimpleButton _buttonBack;

        [Space] [Header("Indication")]
        [SerializeField] private PageIndicator _pageIndicator;

        private TutorialConfig _tutorialConfig;

        private int _currentModuleIndex;

        protected override void Initialize()
        {
            _currentModuleIndex = 0;
            SetModule(_currentModuleIndex);
            _pageIndicator.Construct(_tutorialConfig.ModulesLength + 1);
            _pageIndicator.PageIndexUpdated += OnPageUpdated;
            _buttonBack.Clicked += Back;
            _buttonNext.Clicked += Next;
        }

        protected override void Cleanup()
        {
            _pageIndicator.PageIndexUpdated -= OnPageUpdated;
            _buttonBack.Clicked -= Back;
            _buttonNext.Clicked -= Next;
        }

        public void Construct(IStaticDataService staticData) =>
            _tutorialConfig = staticData.GetTutorialConfig();

        private void Next() =>
            SetModule(NextIndex());

        private void Back() =>
            SetModule(PreviousIndex());

        private int PreviousIndex() =>
            (--_currentModuleIndex).ClampRound(0, _tutorialConfig.ModulesLength);

        private int NextIndex() =>
            (++_currentModuleIndex).ClampRound(0, _tutorialConfig.ModulesLength);

        private void SetModule(int moduleIndex)
        {
            ApplyModulePayload(_tutorialConfig.GetModule(moduleIndex));
            UpdateIndication(moduleIndex);
        }

        private void ApplyModulePayload(TutorialModule module)
        {
            _image.sprite = module.Sprite;
            _textSetter.SetText(module.ExplanationText.TranslateTerm());
        }

        private void UpdateIndication(int index) =>
            _pageIndicator.SetPage(index);

        private void OnPageUpdated(int index)
        {
            _currentModuleIndex = index;
            SetModule(index);
        }
    }
}