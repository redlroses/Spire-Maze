using System;
using UnityEngine;

namespace CodeBase.Logic.HealthEntity
{
    public class PlayerHealth : MonoBehaviour, IDamagable, IHealable, IImmune
    {
        [SerializeField] private int _currentPoints;
        [SerializeField] private int _maxPoints;

        public event Action Died;
        public event Action<int, DamageType> Damaged;
        public event Action<int> Healed;

        public int Points => _currentPoints;
        public int MaxPoints => _maxPoints;
        public bool IsAlive => _currentPoints >= 0;

        public bool IsImmune { get; set; }

        public void Init(int maxPoints, int currentPoints)
        {
            _maxPoints = maxPoints;
            _currentPoints = currentPoints;
        }

        public void Damage(int damagePoints, DamageType damageType)
        {
            Validate(damagePoints);

            if (IsImmune)
            {
                return;
            }

            int newPoints = _currentPoints - damagePoints;

            if (newPoints <= 0)
            {
                Died?.Invoke();
                newPoints = 0;
            }

            int deltaPoints = _currentPoints - newPoints;
            Damaged?.Invoke(deltaPoints, damageType);
            _currentPoints = newPoints;

            Debug.Log($"Damaged: {deltaPoints}, current health: {_currentPoints}");
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