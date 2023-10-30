﻿using System.Diagnostics;
using AYellowpaper.SerializedCollections;
using CodeBase.StaticData;
using NaughtyAttributes;
using TheraBytes.BetterUi;
using UnityEngine;

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