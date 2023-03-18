using System;
using UnityEngine;

namespace CodeBase.Logic.HealthEntity
{
    public sealed class PlayerHealth : Health, IHealable, IImmune
    {
        [SerializeField] private int _currentPoints;
        [SerializeField] private int _maxPoints;

        public event Action<int, DamageType> Damaged;
        public event Action<int> Healed;

        public bool IsImmune { get; set; }

        protected override void OnDamaged(int deltaPoints, DamageType damageType)
        {
            Damaged?.Invoke(deltaPoints, damageType);
        }

        public void Heal(int healPoints)
        {
            Validate(healPoints);

            int newPoints = _currentPoints + healPoints;

            if (newPoints > _maxPoints)
            {
                newPoints = _maxPoints;
            }

            int deltaPoints = newPoints - _currentPoints;
            Healed?.Invoke(deltaPoints);
            _currentPoints = newPoints;

            Debug.Log($"Healed: {deltaPoints}, current health: {_currentPoints}");
        }

        private void Validate(int points)
        {
            if (IsAlive == false)
            {
                throw new InvalidOperationException($"Entity {name} already dead");
            }

            if (points <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(points), "Points must be non negative");
            }
        }
    }
}