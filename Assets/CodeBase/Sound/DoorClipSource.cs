using CodeBase.Logic.DoorEnvironment;
using UnityEngine;

namespace CodeBase.Sound
{
    public class DoorClipSource : AudioClipSource
    {
        [SerializeField] private DoorAnimator _doorAnimator;
        [SerializeField] private AudioClip _audioClip;

        private void OnEnable() =>
            _doorAnimator.Activated += OnOpened;

        private void OnDisable() =>
            _doorAnimator.Activated -= OnOpened;

        private void OnOpened() =>
            PlayOneShot(_audioClip);
    }
}