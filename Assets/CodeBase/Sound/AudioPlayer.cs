using UnityEngine;

namespace CodeBase.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] private AudioSource _audioSource;

        private void OnValidate() =>
            _audioSource ??= GetComponent<AudioSource>();

        public void Play(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void PlayOneShot(AudioClip clip) =>
            _audioSource.PlayOneShot(clip);

        public void PlayOneShot(AudioClip clip, float volume) =>
            _audioSource.PlayOneShot(clip, volume);

        public void Stop() =>
            _audioSource.Stop();

        public void EnableLoop() =>
            _audioSource.loop = true;

        public void DisableLoop() =>
            _audioSource.loop = false;

        public void SetVolume(float volume) =>
            _audioSource.volume = Mathf.Clamp01(volume);
    }
}