using CodeBase.Logic.ChestItem;
using UnityEngine;

namespace CodeBase.Sound
{
    public class ChestSound: AudioClipSource
    {
        [SerializeField] private Chest _chest;
        [SerializeField] private AudioClip _openSound;

        private void OnEnable()
        {
            _chest.Opened += OnOpened;
        }

        private void OnDisable()
        {
            _chest.Opened -= OnOpened;
        }

        private void OnOpened()
        {
            PlayOneShot(_openSound);
        }
    }
}