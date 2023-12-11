using System.Collections.Generic;
using System.Diagnostics;
using CodeBase.Services.Input;
using CodeBase.StaticData;
using CodeBase.Tools.Extension;
using CodeBase.UI.Elements;
using CodeBase.UI.Elements.Buttons;
using JetBrains.Annotations;
using NaughtyAttributes;
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

        private TutorialConfig _config;
        private IReadOnlyCollection<TutorialTrigger> _triggers;
        private int _nextPanelIndex = -1;
        private IInputService _inputService;

        private void OnValidate() =>
            _triggers ??= GetComponentsInChildren<TutorialTrigger>();

        private void OnDestroy() =>
            Unsubscribe();

        public void Construct(IInputService inputService, TutorialConfig config, IReadOnlyCollection<TutorialTrigger> triggers)
        {
            _inputService = inputService;
            _config = config;
            _triggers = triggers;
            Subscribe();
        }

        private void ShowNext()
        {
            if (TryApplyContentFromModule())
            {
                _inputService.DisableMovementMap();
                _panel.Show();
            }
        }

        private bool TryApplyContentFromModule()
        {
            if (++_nextPanelIndex > _config.ModulesLength - 1)
                return false;

            TutorialModule module = _config.GetModule(_nextPanelIndex);
            _image.sprite = module.Sprite;
            _text.SetText(module.ExplanationText.TranslateTerm());

            return true;
        }

        private void Subscribe()
        {
            foreach (TutorialTrigger trigger in _triggers)
                trigger.Triggered += ShowNext;

            _hideButton.Clicked += () =>
            {
                _panel.Hide();
                _inputService.EnableMovementMap();
            };
        }

        private void Unsubscribe()
        {
            foreach (TutorialTrigger trigger in _triggers)
                trigger.Triggered -= ShowNext;

            _hideButton.Cleanup();
        }

        [Button] [Conditional("UNITY_EDITOR")] [UsedImplicitly]
        private void ForceShowNextPanel() =>
            ShowNext();
    }
}