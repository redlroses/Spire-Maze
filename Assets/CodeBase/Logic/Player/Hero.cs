using System;
using CodeBase.Logic.HealthEntity;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Logic.Player
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private HeroAnimator _animator;

        private void Awake()
        {
            _playerHealth ??= GetComponentInChildren<PlayerHealth>();
            _input ??= GetComponent<PlayerInput>();
            _animator ??= GetComponent<HeroAnimator>();
        }

        private void OnEnable()
        {
            _playerHealth.Died += OnDied;
        }

        private void OnDisable()
        {
            _playerHealth.Died -= OnDied;
        }

        private void OnDied()
        {
            _input.enabled = false;
            _animator.PlayDied();
        }
    }
}