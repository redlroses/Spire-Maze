﻿using CodeBase.DelayRoutines;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Tools;
using CodeBase.Tools.Extension;
using UnityEngine;
using UnityEngine.Audio;

namespace CodeBase.Services.Sound
{
    public class SoundService : ISoundService
    {
        private const string MusicVolumeProperty = "Music";
        private const string SfxVolumeProperty = "SFX";
        private const string MasterVolumeProperty = "Master";

        private const float VolumeStep = 0.05f;
        private const float MinVolume = 0.0001f;
        private const float MaxVolume = 1f;
        private const float MaxDecibel = 20f;
        private const float LogarithmBase = 10f;

        private readonly IPersistentProgressService _progressService;
        private readonly AudioMixer _mixer;

        private readonly RoutineSequence _smoothMute;
        private readonly RoutineSequence _smoothUnmute;

        public SoundService(IAssetProvider assets, IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _mixer = assets.LoadAsset<AudioMixer>(AssetPath.AudioMixer);

            _smoothMute = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .WaitUntil(TryDecreaseVolume);

            _smoothUnmute = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .WaitUntil(TryIncreaseVolume);

            WebFocusObserver.InBackgroundChangeEvent += OnInBackgroundChanged;
        }

        public float SfxVolumeLoaded => _progressService.Progress.GlobalData.SoundVolume.Sfx;

        public float MusicVolumeLoaded => _progressService.Progress.GlobalData.SoundVolume.Music;

        public void Load()
        {
            SetMusicVolume(MusicVolumeLoaded);
            SetSfxVolume(SfxVolumeLoaded);
        }

        public void SetMusicVolume(float volume)
        {
            _progressService.Progress.GlobalData.SoundVolume.Music = volume;
            _mixer.SetFloat(MusicVolumeProperty, Mathf.Log10(
                Mathf.Clamp(volume, MinVolume, MaxVolume)) * MaxDecibel);
        }

        public void SetSfxVolume(float volume)
        {
            _progressService.Progress.GlobalData.SoundVolume.Sfx = volume;
            _mixer.SetFloat(SfxVolumeProperty, Mathf.Clamp(volume, MinVolume, MaxVolume).ToDecibels());
        }

        public void SetMasterVolume(float volume)
        {
            _progressService.Progress.GlobalData.SoundVolume.Sfx = volume;
            _mixer.SetFloat(MasterVolumeProperty, Mathf.Clamp(volume, MinVolume, MaxVolume).ToDecibels());
        }

        public void Mute(bool isSmooth = false)
        {
            _smoothUnmute.Stop();

            if (isSmooth)
            {
                _smoothMute.Play();
                return;
            }

            SetMasterVolume(0f);
        }

        public void Unmute(bool isSmooth = false)
        {
            _smoothMute.Stop();

            if (isSmooth)
            {
                _smoothUnmute.Play();
                return;
            }

            SetMasterVolume(1f);
        }

        private bool TryDecreaseVolume()
        {
            _mixer.GetFloat(MasterVolumeProperty, out float volume);
            volume = volume.NormalizeDecibels();
            volume -= VolumeStep;
            SetMasterVolume(Mathf.Clamp01(volume));
            return volume <= 0f;
        }

        private bool TryIncreaseVolume()
        {
            _mixer.GetFloat(MasterVolumeProperty, out float volume);
            volume = volume.NormalizeDecibels();
            volume += VolumeStep;
            SetMasterVolume(Mathf.Clamp01(volume));
            return volume >= 1f;
        }

        private void OnInBackgroundChanged(bool isHidden)
        {
            if (isHidden)
            {
                Mute();
            }
            else
            {
                Unmute();
            }
        }
    }
}