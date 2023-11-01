using CodeBase.Logic.Сollectible;
using UnityEngine;

namespace CodeBase.Sound
{
    public class CollectableSourceClip : AudioClipSource
    {
        [SerializeField] private Collectible _collectible;
        [SerializeField] private AudioClip _openSound;

        private void OnEnable()
        {
            _collectible.Collected += OnOpened;
        }

        private void OnDisable()
        {
            _collectible.Collected -= OnOpened;
        }

        private void OnOpened()
        {
            PlayOneShot(_openSound);
        }
    }
}