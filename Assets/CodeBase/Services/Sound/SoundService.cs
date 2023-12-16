using System;
using CodeBase.DelayRoutines;
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

        private readonly IPersistentProgressService _progressService;
        private readonly AudioMixer _mixer;

        private readonly RoutineSequence _smoothMute;
        private readonly RoutineSequence _smoothUnmute;

        private Locker _cachedLocker;

        public SoundService(IAssetProvider assets, IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _mixer = assets.LoadAsset<AudioMixer>(AssetPath.AudioMixer);

            _smoothMute = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .WaitUntil(TryDecreaseVolume);

            _smoothUnmute = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .WaitUntil(TryIncreaseVolume).Then(() => _cachedLocker = null);

            WebFocusObserver.InBackgroundChangeEvent += OnInBackgroundChanged;
        }

        public float SfxDefaultVolume => _progressService.Progress.GlobalData.SoundVolume.Sfx;

        public float MusicDefaultVolume => _progressService.Progress.GlobalData.SoundVolume.Music;

        public bool IsMuted => _mixer.GetFloat(MasterVolumeProperty, out float volume) && volume.NormalizeDecibels() <= MinVolume + float.Epsilon;

        public void Load()
        {
            SetMusicVolume(MusicDefaultVolume);
            SetSfxVolume(SfxDefaultVolume);
        }

        public void SetMusicVolume(float volume)
        {
            _progressService.Progress.GlobalData.SoundVolume.Music = volume;
            SetVolume(MusicVolumeProperty, volume);
        }

        public void SetSfxVolume(float volume)
        {
            _progressService.Progress.GlobalData.SoundVolume.Sfx = volume;
            SetVolume(SfxVolumeProperty, volume);
        }

        public void Mute(bool isSmooth = false, Locker locker = null)
        {
            if (_cachedLocker is not null)
                return;

            _cachedLocker = locker;

            _smoothUnmute.Stop();

            if (isSmooth)
            {
                _smoothMute.Play();
                return;
            }

            SetMasterVolume(0f);
        }

        public void Unmute(bool isSmooth = false, Locker locker = null)
        {
            if (_cachedLocker != locker)
                return;

            _smoothMute.Stop();

            if (isSmooth)
            {
                _smoothUnmute.Play();
                return;
            }

            SetMasterVolume(1f);
            _cachedLocker = null;
        }

        private void SetMasterVolume(float volume) =>
            SetVolume(MasterVolumeProperty, volume);

        private void SetVolume(string channelName, float volume) =>
            _mixer.SetFloat(channelName, Mathf.Clamp(volume, MinVolume, MaxVolume).ToDecibels());

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
                Mute();
            else
                Unmute();
        }
    }
}