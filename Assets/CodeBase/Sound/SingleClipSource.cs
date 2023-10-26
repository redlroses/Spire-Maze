using UnityEngine;

namespace CodeBase.Sound
{
    public class SingleClipSource : AudioClipSource
    {
        [SerializeField] private AudioClip _clip;

        public void Play() =>
            PlayOneShot(_clip);
    }
}