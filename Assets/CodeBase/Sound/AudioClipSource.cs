using AYellowpaper;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.Sound
{
    public class AudioClipSource : MonoBehaviour
    {
        [SerializeField] [Label("EnableAutoSearch")] private bool _isEnableAutoSearch = true;
        [SerializeField] [HideIf(nameof(_isEnableAutoSearch))]
        private InterfaceReference<IAudioPlayer, MonoBehaviour> _player;

        private void OnValidate()
        {
            if (_isEnableAutoSearch == false)
                return;

            if (_player == null)
                return;

            _player.Value ??= GetComponentInParent<IAudioPlayer>();
        }

        protected void PlayOneShot(AudioClip clip) =>
            _player.Value.PlayOneShot(clip);

        protected void Play(AudioClip clip, bool isLoop)
        {
            _player.Value.Play(clip);

            if (isLoop)
                _player.Value.EnableLoop();
        }

        protected void StopPlaying()
        {
            _player.Value.Stop();
            _player.Value.DisableLoop();
        }

        protected void SetVolume(float volume) =>
            _player.Value.SetVolume(volume);
    }
}