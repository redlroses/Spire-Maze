using CodeBase.Logic.Trap;
using UnityEngine;

namespace CodeBase.Sound
{
    public class RockSound : AudioClipSource
    {
        [SerializeField] private Rock _rockTrap;
        [SerializeField] private AudioClip _activationClip;
        [SerializeField] private AudioClip _destroyingClip;
        [SerializeField] private AudioClip _fragmentationClip;
        [SerializeField] private AudioSource _rollingSound;

        private void OnEnable()
        {
            _rockTrap.TrapActivator.Activated += OnActivated;
            _rockTrap.Destroyed += OnDestroyed;
        }

        private void OnDisable()
        {
            _rockTrap.TrapActivator.Activated -= OnActivated;
            _rockTrap.Destroyed += OnDestroyed;
        }

        private void OnDestroyed()
        {
            _rollingSound.Stop();
            PlayOneShot(_destroyingClip);
            PlayOneShot(_fragmentationClip);
        }

        private void OnActivated()
        {
            _rollingSound.Play();
            PlayOneShot(_activationClip);
        }
    }
}