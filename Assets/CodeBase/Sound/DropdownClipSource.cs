using CodeBase.StaticData;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.Sound
{
    public class DropdownClipSource : AudioClipSource
    {
        [SerializeField] private BetterTextMeshProDropdown _dropdown;
        [SerializeField] private UiSoundConfig _clipConfig;

        private bool _wasPressed;

        private void OnEnable() =>
            _dropdown.StateChanged += OnStateChanged;

        private void OnDisable() =>
            _dropdown.StateChanged -= OnStateChanged;

        private void OnStateChanged(int state)
        {
            if (_wasPressed)
            {
                if (state == (int)SelectionState.Highlighted)
                {
                    _wasPressed = false;

                    return;
                }
            }

            if ((SelectionState)state == SelectionState.Shown)
                _wasPressed = true;

            if (_clipConfig.DropdownClips.TryGetValue((SelectionState)state, out AudioClip clip))
                PlayOneShot(clip);
        }
    }
}