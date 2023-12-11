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
        [SerializeField] private SliderSetter _sliderSetter;

        private TutorialConfig _tutorialConfig;
        private int _currentModuleIndex;

        public void Construct(IStaticDataService staticData) =>
            _tutorialConfig = staticData.GetTutorialConfig();

        protected override void Initialize()
        {
            _currentModuleIndex = 0;
            SetModule(_currentModuleIndex);
            _buttonBack.Clicked += Back;
            _buttonNext.Clicked += Next;
        }

        private void Next() =>
            SetModule(_currentModuleIndex++);

        private void Back() =>
            SetModule(_currentModuleIndex--);

        private void SetModule(int moduleIndex)
        {
            moduleIndex.ClampRound(0, _tutorialConfig.ModulesLength);
            ApplyModulePayload(_tutorialConfig.GetModule(moduleIndex));
            UpdateIndication(moduleIndex);
        }

        private void ApplyModulePayload(TutorialModule module)
        {
            _image.sprite = module.Sprite;
            _textSetter.SetText(module.ExplanationText);
        }

        private void UpdateIndication(int index) =>
            _sliderSetter.SetNormalizedValue(index / (float) _tutorialConfig.ModulesLength);
    }
}