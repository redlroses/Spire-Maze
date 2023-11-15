using CodeBase.Logic.Checkpoint;
using UnityEngine;

namespace CodeBase.Sound
{
    public class SavepointClipSource : AudioClipSource
    {
        [SerializeField] private Savepoint _savepoint;
        [SerializeField] private AudioClip _activationClip;
        [SerializeField] private AudioClip _fireIgnitionClip;

        private void OnEnable()
        {
            _savepoint.Activated += OnActivated;
        }

        private void OnDisable()
        {
            _savepoint.Activated -= OnActivated;
        }

        private void OnActivated(bool isTriggerActivation)
        {
            if (isTriggerActivation)
            {
                PlayOneShot(_activationClip);
            }

            PlayOneShot(_fireIgnitionClip);
        }
    }
}