using System;

namespace CodeBase.Data
{
    [Serializable]
    public class SoundVolume
    {
        private const float DefaultVolume = 0.5f;

        public float Music;
        public float Sfx;

        public void Reset()
        {
            Music = DefaultVolume;
            Sfx = DefaultVolume;
        }
    }
}