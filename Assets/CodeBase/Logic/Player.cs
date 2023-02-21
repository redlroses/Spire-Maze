using System;
using UnityEngine;
using NTC.Global.Cache;

namespace CodeBase.Logic
{
    public class Player : MonoCache
    {
        private int _maxHealth;
        private int _currentHealth;
        private bool _isDie;

        public event Action Die;

        public void ReceiveDamage(int damage)
        {
            if (damage < 0)
                throw new InvalidOperationException($"{GetType()}: ReceiveDamage(int damage): invalid value received damage = {damage}");

            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                _isDie = true;
                Die?.Invoke();
            }
        }

        private void Heal(int health)
        {
            if (health < 0)
                throw new InvalidOperationException($"{GetType()}: Heal(int health): invalid value incoming health = {health}");

            _currentHealth = Mathf.Clamp(_currentHealth + health, 0, _maxHealth);
        }
    }
}