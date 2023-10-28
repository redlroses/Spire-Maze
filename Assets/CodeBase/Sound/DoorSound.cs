using CodeBase.Logic.DoorEnvironment;
using UnityEngine;

namespace CodeBase.Sound
{
    public class DoorSound:AudioClipSource
    {
        [SerializeField] private Door _door;
        [SerializeField] private AudioClip _audioClip;

        private void OnEnable()
        {
            _door.Opened += OnOpened;
        }

        private void OnDisable()
        {
            _door.Opened -= OnOpened;
        }

        private void OnOpened()
        {
            PlayOneShot(_audioClip);
        }
    }
}