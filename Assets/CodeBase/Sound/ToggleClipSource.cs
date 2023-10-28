using AYellowpaper.SerializedCollections;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.Sound
{
    public class ToggleClipSource : AudioClipSource
    {
        [SerializeField] private BetterToggle _toggle;
        [SerializeField] private SerializedDictionary<SelectionState, AudioClip> _clips;
        
        private void OnEnable()
        {
            _toggle.StateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            _toggle.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(int state)
        {
            if (_clips.TryGetValue((SelectionState) state, out AudioClip clip))
                PlayOneShot(clip);
        }
    }
}