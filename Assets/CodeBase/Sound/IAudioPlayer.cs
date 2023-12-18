using UnityEngine;

namespace CodeBase.Sound
{
    public interface IAudioPlayer
    {
        void PlayOneShot(AudioClip clip);

        void PlayOneShot(AudioClip clip, float volume);

        void Stop();

        void EnableLoop();

        void DisableLoop();

        void Play(AudioClip clip);

        void SetVolume(float volume);
    }
}