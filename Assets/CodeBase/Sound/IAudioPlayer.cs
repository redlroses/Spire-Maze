using UnityEngine;

namespace CodeBase.Sound
{
    public interface IAudioPlayer
    {
        void PlayOneShot(AudioClip clip);
        void PlayOneShot(AudioClip clip, float volume);
    }
}