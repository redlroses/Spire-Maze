using System;

namespace CodeBase.Services.Sound
{
    public class SoundService : ISoundService
    {
        public void MusicVolume(float volume)
        {
#if !UNITY_EDITOR
            throw new System.NotImplementedException();
#endif
        }

        public void SoundVolume(float volume)
        {
#if !UNITY_EDITOR
            throw new System.NotImplementedException();
#endif
        }

        public void Mute()
        {
#if !UNITY_EDITOR
            throw new NotImplementedException();
#endif
        }

        public void UnMute()
        {
#if !UNITY_EDITOR
            throw new System.NotImplementedException();
#endif
        }
    }
}