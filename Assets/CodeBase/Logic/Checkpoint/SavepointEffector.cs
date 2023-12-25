using NTC.Global.System;
using UnityEngine;

namespace CodeBase.Logic.Checkpoint
{
    public class SavepointEffector : MonoBehaviour
    {
        [SerializeField] private Savepoint _savepoint;
        [SerializeField] private ParticleSystem _fire;

        private void OnEnable()
        {
            _savepoint.Activated += OnActivated;
        }

        private void OnDisable()
        {
            _savepoint.Activated -= OnActivated;
        }

        private void OnActivated(bool isActivated)
        {
            _fire.gameObject.Enable();
            _fire.Play();
        }
    }
}