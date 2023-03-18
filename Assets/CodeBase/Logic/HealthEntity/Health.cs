using System;
using UnityEngine;

namespace CodeBase.Logic.HealthEntity
{
    public class Health : MonoBehaviour, IHealth
    {
        [SerializeField] private int _currentPoints;
        [SerializeField] private int _maxPoints;

        public event Action Died;

        public int Points => _currentPoints;
        public int MaxPoints => _maxPoints;
        public bool IsAlive => _currentPoints >= 0;

        public void Init(int maxPoints, int currentPoints)
        {
            _maxPoints = maxPoints;
            _currentPoints = currentPoints;
        }

        public void Damage(int damagePoints, DamageType damageType)
        {
            Validate(damagePoints);

            int newPoints = _currentPoints - damagePoints;

            if (newPoints <= 0)
            {
                Died?.Invoke();
                newPoints = 0;
            }

            int deltaPoints = _currentPoints - newPoints;
            _currentPoints = newPoints;

            OnDamaged(deltaPoints, damageType);

            Debug.Log($"Damaged: {deltaPoints}, current health: {_currentPoints}");
        }

        protected virtual void OnDamaged(int deltaPoints, DamageType damageType)
        {
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