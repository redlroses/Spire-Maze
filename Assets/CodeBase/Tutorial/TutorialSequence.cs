using CodeBase.Services.Input;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Elements.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Tutorial
{
    public class TutorialSequence : MonoBehaviour
    {
        [Header("Panel Settings")]
        [SerializeField] private FadeOutPanel _panel;
        [SerializeField] private SimpleButton _hideButton;
        [SerializeField] private Image _image;
        [SerializeField] private TextSetter _text;

        [Space] [Header("Content Settings")]
        [SerializeField] private TutorialTrigger[] _triggers;
        [SerializeField] private TutorialConfig _tutorialConfig;

        private int _nextPanelIndex;
        private IInputService _inputService;

        private void OnValidate() =>
            _triggers ??= GetComponentsInChildren<TutorialTrigger>();

        private void Start() =>
            Subscribe();

        private void OnDestroy() =>
            Unsubscribe();

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _hideButton.Subscribe(_inputService.EnableMovementMap);
        }

        private void ShowNext()
        {
            _inputService.DisableMovementMap();
            ApplyContentFromModule();
            _panel.Show();
        }

        private void ApplyContentFromModule()
        {
            TutorialModule module = _tutorialConfig.GetModule(_nextPanelIndex++);
            _image.sprite = module.Sprite;
            _text.SetText(module.ExplanationText);
        }

        private void Subscribe()
        {
            foreach (TutorialTrigger trigger in _triggers)
                trigger.Triggered += ShowNext;
        }

        private void Unsubscribe()
        {
            foreach (TutorialTrigger trigger in _triggers)
                trigger.Triggered -= ShowNext;

            _hideButton.Cleanup();
        }
    }
}