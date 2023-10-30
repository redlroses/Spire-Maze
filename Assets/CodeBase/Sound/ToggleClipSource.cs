using System.Diagnostics;
using CodeBase.StaticData;
using NaughtyAttributes;
using TheraBytes.BetterUi;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CodeBase.Sound
{
    public class ToggleClipSource : AudioClipSource
    {
        [SerializeField] private BetterToggle _toggle;
        [SerializeField] private UiSoundConfig _clipConfig;

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
            Debug.Log($"State: {state}");
            
            if (_clipConfig.ToggleClips.TryGetValue((SelectionState) state, out AudioClip clip))
                PlayOneShot(clip);
        }

        [Conditional("UNITY_EDITOR")] [Button]
        public void LoadConfig()
        {
            _clipConfig = Resources.Load<UiSoundConfig>("StaticData/UiSoundConfig");
        }
    }
}