using System;
using System.Collections;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Player
{
    public class Hero : MonoCache
    {
        [SerializeField] private float _durationInvulnerability;
        [SerializeField] private ParticleSystem _invulnerabilityEffect;

        private Coroutine _invulnerabilityActive;
        private WaitForSeconds _waitForSeconds;
        private int _maxHealth = 100;
        private int _currentHealth;
        private bool _isInvulnerabilityActive;
        private bool _isDie;

        public event Action Die;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _waitForSeconds = new WaitForSeconds(_durationInvulnerability);
        }

        public void ReceiveDamage(int damage)
        {
            if (_isInvulnerabilityActive == true)
                return;

            if (damage < 0)
                throw new InvalidOperationException($"{GetType()}: ReceiveDamage(int damage): invalid value received damage = {damage}");

            _currentHealth -= damage;
            Debug.Log($"Damage: {damage}; Current Health: {_currentHealth}");

            if (_currentHealth <= 0)
            {
                _isDie = true;
                Die?.Invoke();
                Debug.Log($"is die: {_isDie}");
            }
            else
            {
                _invulnerabilityActive = RestartCoroutine(ActiveInvulnerability());
            }
        }

        public void Heal(int health)
        {
            if (health < 0)
                throw new InvalidOperationException($"{GetType()}: Heal(int health): invalid value incoming health = {health}");

            _currentHealth = Mathf.Clamp(_currentHealth + health, 0, _maxHealth);
            Debug.Log($"Heal: {health}; Current Health: {_currentHealth}");
        }

        private Coroutine RestartCoroutine(IEnumerator coroutine)
        {
            if (_invulnerabilityActive != null)
            {
                StopCoroutine(coroutine);
                _invulnerabilityActive = null;
            }

            return StartCoroutine(coroutine);
        }

        private IEnumerator ActiveInvulnerability()
        {
            _isInvulnerabilityActive = true;
            _invulnerabilityEffect.Play();
            Debug.Log("Invulnerabilety");
            yield return _waitForSeconds;

            _isInvulnerabilityActive = false;
            _invulnerabilityEffect.Stop();
            Debug.Log("Vulnerable");
        }
    }
}