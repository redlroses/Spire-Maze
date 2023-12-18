using CodeBase.StaticData;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.Sound
{
    public class ButtonClipSource : AudioClipSource
    {
        [SerializeField] private BetterButton _button;
        [SerializeField] private UiSoundConfig _clipConfig;

        private void OnEnable() =>
            _button.StateChanged += OnStateChanged;

        private void OnDisable() =>
            _button.StateChanged -= OnStateChanged;

        private void OnStateChanged(int state)
        {
            if (_clipConfig.ButtonClips.TryGetValue((SelectionState)state, out AudioClip clip))
                PlayOneShot(clip);
        }
    }
}