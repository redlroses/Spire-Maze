using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.Trap;
using UnityEngine;

namespace CodeBase.Sound
{
    public class TrapSound : AudioClipSource
    {
        [SerializeField] private Trap _trap;
        [SerializeField] private AudioClip _audioClip;

        private void OnEnable()
        {
            _trap.TrapActivator.Activated += OnActivated;
        }

        private void OnDisable()
        {
            _trap.TrapActivator.Activated -= OnActivated;
        }

        private void OnActivated(IDamagable obj)
        {
            PlayOneShot(_audioClip);
        }
    }
}