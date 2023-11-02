using CodeBase.Logic.Observer;
using UnityEngine;

namespace CodeBase.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer3D : ObserverTargetExited<DistantAudioListenerObserver, IDistantAudioListener>, IAudioPlayer
    {
        [SerializeField] private AudioSource _audioSource;

        protected override void OnValidate()
        {
            base.OnValidate();
            _audioSource ??= GetComponent<AudioSource>();
        }

        protected override void OnTriggerObserverEntered(IDistantAudioListener target)
        {
        }

        protected override void OnTriggerObserverExited(IDistantAudioListener target)
        {
            throw new System.NotImplementedException();
        }

        public void PlayOneShot(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }

        public void PlayOneShot(AudioClip clip, float volume)
        {
            _audioSource.PlayOneShot(clip, volume);
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}