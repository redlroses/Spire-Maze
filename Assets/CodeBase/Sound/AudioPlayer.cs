using UnityEngine;

namespace CodeBase.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] private AudioSource _audioSource;

        private void OnValidate()
        {
            _audioSource ??= GetComponent<AudioSource>();
        }

        public void PlayOneShot(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }

        public void PlayOneShot(AudioClip clip, float volume)
        {
            _audioSource.PlayOneShot(clip, volume);
        }
    }
}