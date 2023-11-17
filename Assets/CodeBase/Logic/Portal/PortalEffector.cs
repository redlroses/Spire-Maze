using System;
using AYellowpaper;
using UnityEngine;

namespace CodeBase.Logic.Portal
{
    public class PortalEffector : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IActivated, MonoBehaviour> _portal;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private AudioSource _audioSource;

        private void OnValidate()
        {
            if (_portal == null)
            {
                _portal = new InterfaceReference<IActivated, MonoBehaviour>
                {
                    UnderlyingValue = GetComponent<LevelTransfer>(),
                    Value = GetComponent<LevelTransfer>()
                };
            }

            _effect ??= GetComponentInChildren<ParticleSystem>();
            _audioSource ??= GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _effect.Stop();
            _audioSource.Stop();
            _portal.Value.Activated += OnActivated;
        }

        private void OnDisable()
        {
            _portal.Value.Activated -= OnActivated;
        }

        private void OnActivated()
        {
            _effect.Play();
            _audioSource.Play();
        }
    }
}