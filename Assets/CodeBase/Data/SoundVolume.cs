using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
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