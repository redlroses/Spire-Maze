using CodeBase.Logic.Trap;
using UnityEngine;

namespace CodeBase.Sound
{
    public class TrapSourceSlip : AudioClipSource
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

        private void OnActivated()
        {
            PlayOneShot(_audioClip);
        }
    }
}