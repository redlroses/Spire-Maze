using CodeBase.DelayRoutines;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.Audio;

namespace CodeBase.Services.Sound
{
    public class SoundService : ISoundService
    {
        private const string MusicVolumeProperty = "Music";
        private const string SfxVolumeProperty = "SFX";
        private const float VolumeStep = 0.05f;
        private const float MinVolume = -80f;
        private const float MaxVolume = 20f;

        private readonly IPersistentProgressService _progressService;
        private readonly AudioMixer _mixer;

        private readonly RoutineSequence _smoothMute;
        private readonly RoutineSequence _smoothUnmute;

        public float SfxVolume => _progressService.Progress.GlobalData.SoundVolume.Sfx;
        public float MusicVolume => _progressService.Progress.GlobalData.SoundVolume.Music;

        public SoundService(IAssetProvider assets, IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _mixer = assets.LoadAsset<AudioMixer>(AssetPath.AudioMixer);

            _smoothMute = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .WaitUntil(TryDecreaseVolume)
                .Then(() => AudioListener.pause = true);

            _smoothUnmute = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .Then(() => AudioListener.pause = false)
                .WaitUntil(TryIncreaseVolume);
        }

        public void Load()
        {
            SetMusicVolume(MusicVolume);
            SetSfxVolume(SfxVolume);
        }

        public void SetMusicVolume(float volume)
        {
            _progressService.Progress.GlobalData.SoundVolume.Music = volume;
            _mixer.SetFloat(MusicVolumeProperty, Mathf.Lerp(MinVolume, MaxVolume, volume));
        }

        public void SetSfxVolume(float volume)
        {
            _progressService.Progress.GlobalData.SoundVolume.Sfx = volume;
            _mixer.SetFloat(SfxVolumeProperty, Mathf.Lerp(MinVolume, MaxVolume, volume));
        }

        public void Mute(bool isSmooth = false)
        {
            if (isSmooth)
            {
                _smoothMute.Play();
                return;
            }

            AudioListener.pause = true;
            AudioListener.volume = 0f;
        }

        public void Unmute(bool isSmooth = false)
        {
            if (isSmooth)
            {
                _smoothUnmute.Play();
                return;
            }

            AudioListener.pause = false;
            AudioListener.volume = 1f;
        }

        private bool TryDecreaseVolume()
        {
            if (AudioListener.volume <= 0f)
            {
                return true;
            }

            AudioListener.volume -= VolumeStep;
            return false;
        }

        private bool TryIncreaseVolume()
        {
            if (AudioListener.volume >= 0f)
            {
                return true;
            }

            AudioListener.volume += VolumeStep;
            return false;
        }
    }
}