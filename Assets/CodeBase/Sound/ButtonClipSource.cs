using AYellowpaper.SerializedCollections;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.Sound
{
    public class ButtonClipSource : AudioClipSource
    {
        [SerializeField] private BetterButton _button;
        [SerializeField] private SerializedDictionary<SelectionState, AudioClip> _clips;

        private void OnEnable()
        {
            _button.StateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            _button.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(int state)
        {
            if (_clips.TryGetValue((SelectionState) state, out AudioClip clip))
                PlayOneShot(clip);
        }
    }
}